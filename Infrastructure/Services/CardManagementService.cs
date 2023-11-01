using AutoMapper;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Exceptions;
using RapidPayTest.Application.Interfaces.Repositories;
using RapidPayTest.Application.Interfaces.Services;
using RapidPayTest.Application.Wrappers;
using RapidPayTest.Domain.Entities;
using RapidPayTest.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RapidPayTest.Infrastructure.Services
{
    public class CardManagementService : BaseService, ICardManagementService
    {
        private readonly ICardManagementRepository _cardManagementRepository;
        private readonly IRepositoryAsync<Transaction> _transactionRepo;
        private readonly IRepositoryAsync<User> _userRepo;
        private readonly IRepositoryAsync<CardManagement> _cardManagementRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<CardManagementCreateDto> _validator;
        public CardManagementService(ICardManagementRepository cardManagementRepository, IRepositoryAsync<Transaction> transactionRepo, IRepositoryAsync<User> userRepo, IRepositoryAsync<CardManagement> cardManagementRepo, IMapper mapper, IValidator<CardManagementCreateDto> validator, IHttpContextAccessor context) : base(context)
        {
            _cardManagementRepository = cardManagementRepository;
            _transactionRepo = transactionRepo;
            _userRepo = userRepo;
            _cardManagementRepo = cardManagementRepo;
            _mapper = mapper;
            _validator = validator;
        }

        // Inserts a new Card Management record into the system with the provided data.
        public async Task<Response<CardManagementVm>> InsertAsync(CardManagementCreateDto dto)
        {
            var valResult = _validator.Validate(dto);
            if (!valResult.IsValid) throw new ApiValidationException(valResult.Errors);

            User result = _mapper.Map<User>(await _userRepo.WhereAsync(x => x.ID.Equals(dto.IDUser)));
            if (result == null) { throw new ApiException($"No user found with the provided ID: {dto.IDUser}"); }

            var obj = _mapper.Map<CardManagement>(dto);
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = GetLoggerUserId();
            obj.IsDeleted = false;
            obj.Status = "A";
            obj.IDUser = dto.IDUser;
            obj.Balance = dto.Amount;

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepo.AddAsync(obj)));
        }

        public async Task<Response<CardManagementVm>> UpdateAsync(CardManagmentUpdateBalanceDto obj)
        {
            var objDb = _mapper.Map<CardManagement>(await _cardManagementRepo.WhereAsync(x => x.IDUser == GetLoggerUserId()));

            if (objDb == null)
            {
                throw new KeyNotFoundException($"Card Management not found");
            }

            // TAX/Fee
            decimal Fee = (UfeService.Instance.LastFeeAmount * obj.Amount);

            // Set properties of the update CardManagement
            CardManagement cardManagement = new();
            cardManagement = objDb;
            cardManagement.UpdateAt = DateTime.Now;
            cardManagement.UpdateBy = GetLoggerUserId();
            cardManagement.Balance = cardManagement.Balance + (obj.Amount - Fee);

            // Set properties of the update Transanction
            Transaction transaction = new Transaction
            {
                CreateAt = DateTime.Now,
                CreateBy = GetLoggerUserId(),
                IsDeleted = false,
                Status = "A",
                PaymentAmount = (obj.Amount - Fee),
                FeeAmount = Fee,
                CardNumber = objDb.CardNumber,
                TotalPayment = obj.Amount
            };

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepository.UpdateAsync(cardManagement, transaction)));
        }

        // This method retrieves the account balance for the currently logged-in user from the Card Management system.
        public async Task<Response<CardManagementVm>> GetAccountBalance()
        {
            var data = await _cardManagementRepo.WhereAsync(x => x.IDUser == GetLoggerUserId());
            if (data == null)
            {
                throw new KeyNotFoundException($"Card Management not found ");
            }

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(data));
        }

        //This method retrieves a paged list of transactions associated with the currently logged-in user.
        public async Task<PagedResponse<IList<TransactionVm>>> GetTransactionPagedListAsync(int pageNumber, int pageSize, string filter = null)
        {
            List<Expression<Func<Transaction, bool>>> queryFilter = new List<Expression<Func<Transaction, bool>>>();
            queryFilter.Add(x => x.CreateBy.Equals(GetLoggerUserId()));

            var list = await _transactionRepo.GetPagedList(pageNumber, pageSize, queryFilter);
            if (list == null || list.Data.Count == 0)
            {
                throw new KeyNotFoundException($"Transactions not found");
            }

            return new PagedResponse<IList<TransactionVm>>(_mapper.Map<IList<TransactionVm>>(list.Data), list.PageNumber, list.PageSize, list.TotalCount);
        }
    }
}
