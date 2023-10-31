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
    public class PrioridadesService : BaseService, IPrioridadesService
    {
        private readonly IRepositoryAsync<Prioridades> _PrioridadesRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<PrioridadesDto> _validator;
        public PrioridadesService(IRepositoryAsync<Prioridades> PrioridadesRepo, IMapper mapper, IValidator<PrioridadesDto> validator, IHttpContextAccessor context) : base(context)
        {
            _PrioridadesRepo = PrioridadesRepo;
            _mapper = mapper;
            _validator = validator;
        }

        private async Task<bool> ExitsAsync(int Id)
        {
            var result = await _PrioridadesRepo.Exists(x => x.CodPrioridad.Equals(Id));
            if (result) { return true; }
            throw new ApiException("Prioridades not found");
        }

        public async Task<Response<PrioridadesVm>> InsertAsync(PrioridadesDto dto)
        {

            var valResult = _validator.Validate(dto);
            if (!valResult.IsValid) throw new ApiValidationException(valResult.Errors);

            var obj = _mapper.Map<Prioridades>(dto);


            return new Response<PrioridadesVm>(_mapper.Map<PrioridadesVm>(await _PrioridadesRepo.AddAsync(obj)));

        }

        public async Task<Response<PrioridadesVm>> UpdateAsync(int id, PrioridadesDto dto)
        {
            var valResult = _validator.Validate(dto);
            if (!valResult.IsValid) throw new ApiValidationException(valResult.Errors);

            await ExitsAsync(id);

            var objDb = await _PrioridadesRepo.WhereAsync(x => x.CodPrioridad.Equals(id));
            var obj = _mapper.Map<Prioridades>(dto);

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

            return new Response<PrioridadesVm>(_mapper.Map<PrioridadesVm>(await _PrioridadesRepo.UpdateAsync(obj)));
        }

        public async Task<Response<PrioridadesVm>> GetByIdAsync(int id)
        {
            var data = await _PrioridadesRepo.GetByIdAsync(id);
            if (data == null)
            {
                throw new KeyNotFoundException($"Prioridades not found by id={id}");
            }

            return new Response<PrioridadesVm>(_mapper.Map<PrioridadesVm>(data));
        }


        public async Task<PagedResponse<IList<PrioridadesVm>>> GetPagedListAsync(int pageNumber, int pageSize, string filter = null)
        {

            List<Expression<Func<Prioridades, bool>>> queryFilter = new List<Expression<Func<Prioridades, bool>>>();
            List<Expression<Func<Prioridades, Object>>> includes = new List<Expression<Func<Prioridades, Object>>>();


            var user = base.GetLoggerUser();
            var userId = base.GetLoggerUserId();


            if (filter != null || filter.Length > 0)
            {
                queryFilter.Add(x => x.Prioridad.Contains(filter));
            }

            var list = await _PrioridadesRepo.GetPagedList(pageNumber, pageSize, queryFilter, includes: includes);
            if (list == null || list.Data.Count == 0)
            {
                throw new KeyNotFoundException($"Prioridades not found");
            }

            return new PagedResponse<IList<PrioridadesVm>>(_mapper.Map<IList<PrioridadesVm>>(list.Data), list.PageNumber, list.PageSize, list.TotalCount);
        }


    }
}
