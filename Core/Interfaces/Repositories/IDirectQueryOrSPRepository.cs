using System.Threading.Tasks;
using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.DTOs.ViewModel;
using RappidPayTest.Application.Wrappers;

namespace RappidPayTest.Application.Interfaces.Repositories
{
    public interface IDirectQueryOrSPRepository
    {
        Task<Response<SpResult>> SpTestResult(string nombre, string apellido);

    }
}
