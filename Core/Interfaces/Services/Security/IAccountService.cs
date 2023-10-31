using System.Threading.Tasks;
using RappidPayTest.Application.DTOs.Settings;
using RappidPayTest.Application.Wrappers;

namespace RappidPayTest.Application.Interfaces.Services.Security
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
    }
}
