using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.DTOs.ViewModel;
using RappidPayTest.Application.Wrappers;

namespace RappidPayTest.Application.Interfaces.Services
{
    public interface IPrioridadesService
    {
        Task<Response<PrioridadesVm>> InsertAsync(PrioridadesDto dto);
        Task<Response<PrioridadesVm>> UpdateAsync(int id, PrioridadesDto dto);

        Task<Response<PrioridadesVm>> GetByIdAsync(int id);

        Task<PagedResponse<IList<PrioridadesVm>>> GetPagedListAsync(int pageNumber, int pageSize, string filter = null);
    }
}
