using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Wrappers;

namespace RapidPayTest.Application.Interfaces.Services
{
    public interface ICardManagementService
    {
        Task<Response<CardManagementVm>> InsertAsync(CardManagementCreateDto dto);
        Task<Response<CardManagementVm>> UpdateAsync(CardManagmentUpdateBalanceDto obj);
        Task<Response<CardManagementVm>> GetAccountBalance();
        Task<PagedResponse<IList<TransactionVm>>> GetTransactionPagedListAsync(int pageNumber, int pageSize, string filter = null);
    }
}
