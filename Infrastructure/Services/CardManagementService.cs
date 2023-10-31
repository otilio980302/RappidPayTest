using AutoMapper;
using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.DTOs.ViewModel;
using RappidPayTest.Application.Exceptions;
using RappidPayTest.Application.Interfaces.Repositories;
using RappidPayTest.Application.Interfaces.Services;
using RappidPayTest.Application.Wrappers;
using RappidPayTest.Domain.Entities;
using RappidPayTest.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RappidPayTest.Infrastructure.Services
{
    public class CardManagementService : BaseService, ICardManagementService
    {
        private readonly IRepositoryAsync<CardManagement> _cardManagementRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<CardManagementCreateDto> _validator;
        public CardManagementService(IRepositoryAsync<CardManagement> cardManagementRepo, IMapper mapper, IValidator<CardManagementCreateDto> validator, IHttpContextAccessor context) : base(context)
        {
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

            var obj = _mapper.Map<CardManagement>(dto);
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = Convert.ToInt32(base.GetLoggerUserId());
            obj.IsDeleted = false;
            obj.Status = "A";

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepo.AddAsync(obj)));
        }

        public async Task<Response<CardManagementVm>> UpdateAsync(CardManagementDto obj)
        {
            var objDb = await _cardManagementRepo.WhereAsync(x => x.CardNumber.Equals(obj.CardNumber));
            CardManagement cardManagement = new CardManagement();
            cardManagement = objDb;
            cardManagement.Balance = objDb.Balance + obj.Balance;

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepo.UpdateAsync(cardManagement)));
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


        public async Task<PagedResponse<IList<CardManagementVm>>> GetPagedListAsync(int pageNumber, int pageSize, string filter = null)
        {

            List<Expression<Func<CardManagement, bool>>> queryFilter = new List<Expression<Func<CardManagement, bool>>>();
            List<Expression<Func<CardManagement, Object>>> includes = new List<Expression<Func<CardManagement, Object>>>();


            var user = base.GetLoggerUser();
            var userId = base.GetLoggerUserId();


            if (filter != null || filter.Length > 0)
            {
                queryFilter.Add(x => x.CardNumber.Equals(filter));
            }

            var list = await _cardManagementRepo.GetPagedList(pageNumber, pageSize, queryFilter, includes: includes);
            if (list == null || list.Data.Count == 0)
            {
                throw new KeyNotFoundException($"Card Management not found");
            }

            return new PagedResponse<IList<CardManagementVm>>(_mapper.Map<IList<CardManagementVm>>(list.Data), list.PageNumber, list.PageSize, list.TotalCount);
        }


    }
}
