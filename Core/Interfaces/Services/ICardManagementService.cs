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
    public interface ICardManagementService
    {
        Task<Response<CardManagementVm>> InsertAsync(CardManagementCreateDto dto);
        Task<Response<CardManagementVm>> UpdateAsync(CardManagementDto obj);

        Task<Response<CardManagementVm>> GetByCardNumberAsync(string CardNumber);

        Task<PagedResponse<IList<CardManagementVm>>> GetPagedListAsync(int pageNumber, int pageSize, string filter = null);
    }
}
