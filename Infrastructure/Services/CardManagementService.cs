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
        private readonly IValidator<CardManagementDto> _validator;
        public CardManagementService(IRepositoryAsync<CardManagement> cardManagementRepo, IMapper mapper, IValidator<CardManagementDto> validator, IHttpContextAccessor context) : base(context)
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

        public async Task<Response<CardManagementVm>> InsertAsync(CardManagementDto dto)
        {

            var valResult = _validator.Validate(dto);
            if (!valResult.IsValid) throw new ApiValidationException(valResult.Errors);

            var obj = _mapper.Map<CardManagement>(dto);


            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepo.AddAsync(obj)));

        }

        public async Task<Response<CardManagementVm>> UpdateAsync(int id, CardManagementDto dto)
        {
            var valResult = _validator.Validate(dto);
            if (!valResult.IsValid) throw new ApiValidationException(valResult.Errors);

            await ExitsAsync(id);

            var objDb = await _cardManagementRepo.WhereAsync(x => x.CardNumber.Equals(id));
            var obj = _mapper.Map<CardManagement>(dto);

            //Note: You can automap the object or map manualy, as this code down.

            //#region Mapping 
            //obj.Name = dto.Name;
            //obj.LasName = dto.LasName;
            //obj.Address = dto.Address;
            //obj.Status = dto.Status;
            //obj.Note = dto.Note;
            //obj.YearOfbirth = dto.YearOfbirth;
            //obj.MonthOfbirth = dto.MonthOfbirth;
            //obj.DayOfbirth = dto.DayOfbirth;
            //#endregion Mapping

            return new Response<CardManagementVm>(_mapper.Map<CardManagementVm>(await _cardManagementRepo.UpdateAsync(obj)));
        }

        public async Task<Response<CardManagementVm>> GetByIdAsync(int id)
        {
            var data = await _cardManagementRepo.GetByIdAsync(id);
            if (data == null)
            {
                throw new KeyNotFoundException($"Card Management not found by id={id}");
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
