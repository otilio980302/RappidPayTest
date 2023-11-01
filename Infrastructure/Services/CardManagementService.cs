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

        private async Task<bool> ExitsAsync(int Id)
        {
            var result = await _cardManagementRepo.Exists(x => x.ID.Equals(Id));
            if (result) { return true; }
            throw new ApiException("Card Management not found");
        }

        public async Task<Response<CardManagementVm>> InsertAsync(CardManagementCreateDto dto)
        {

            var valResult = _validator.Validate(dto);
            if (!valResult.IsValid) throw new ApiValidationException(valResult.Errors);

            var result = await _userRepo.Exists(x => x.ID.Equals(dto.IDUser));
            if (result) { throw new ApiException($"No user found with the provided ID: {dto.IDUser}"); }

            var obj = _mapper.Map<CardManagement>(dto);
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = Convert.ToInt32(base.GetLoggerUserId());
            obj.IsDeleted = false;
            obj.Status = "A";
            obj.IDUser = dto.IDUser;

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepo.AddAsync(obj)));
        }

        public async Task<Response<CardManagementVm>> UpdateAsync(CardManagementDto obj)
        {
            var objDb = await _cardManagementRepo.WhereAsync(x => x.CardNumber.Equals(obj.CardNumber));

            if (objDb == null)
            {
                throw new KeyNotFoundException($"Card Management not found");
            }

            decimal Fee = (UfeService.Instance.LastFeeAmount * obj.Amount);

            CardManagement cardManagement = new();
            cardManagement = objDb;
            cardManagement.UpdateAt = DateTime.Now;
            cardManagement.UpdateBy = Convert.ToInt32(base.GetLoggerUserId());
            cardManagement.Balance = cardManagement.Balance + (obj.Amount - Fee);

            Transaction transaction = new Transaction
            {
                CreateAt = DateTime.Now,
                CreateBy = Convert.ToInt32(base.GetLoggerUserId()),
                IsDeleted = false,
                Status = "A",
                PaymentAmount = (obj.Amount - Fee),
                FeeAmount = Fee,
                CardNumber = obj.CardNumber,
                TotalPayment = obj.Amount
            };

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepository.UpdateAsync(cardManagement, transaction)));
        }

        public async Task<Response<CardManagementVm>> GetByCardNumberAsync(string CardNumber)
        {
            var data = await _cardManagementRepo.WhereAsync(x => x.CardNumber == CardNumber);
            if (data == null)
            {
                throw new KeyNotFoundException($"Card Management not found by id={CardNumber}");
            }

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(data));
        }

        public async Task<PagedResponse<IList<TransactionVm>>> GetTransactionPagedListAsync(int pageNumber, int pageSize, string filter = null)
        {

            List<Expression<Func<Transaction, bool>>> queryFilter = new List<Expression<Func<Transaction, bool>>>();

            var userId = base.GetLoggerUserId();

            var cardManagement = _mapper.Map<CardManagement>(await _cardManagementRepo.WhereAsync(x => x.IDUser.Equals(userId)));

            if (filter != null || filter.Length > 0)
            {
                queryFilter.Add(x => x.CardNumber.Equals(cardManagement.CardNumber));
            }

            var list = await _transactionRepo.GetPagedList(pageNumber, pageSize, queryFilter);
            if (list == null || list.Data.Count == 0)
            {
                throw new KeyNotFoundException($"Card Management not found");
            }

            return new PagedResponse<IList<TransactionVm>>(_mapper.Map<IList<TransactionVm>>(list.Data), list.PageNumber, list.PageSize, list.TotalCount);
        }
    }
}
