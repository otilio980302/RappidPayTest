using System.Collections.Generic;
using System.Threading.Tasks;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.Settings;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Wrappers;

namespace RapidPayTest.Application.Interfaces.Services.Security
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<UserVm>> RegisterAsync(UserDto request, string origin);
        Task<PagedResponse<IList<UserVm>>> GetUsers(int pageNumber, int pageSize, string filter = "");
    }
}
