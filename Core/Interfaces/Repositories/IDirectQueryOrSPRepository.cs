using System.Threading.Tasks;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Wrappers;

namespace RapidPayTest.Application.Interfaces.Repositories
{
    public interface IDirectQueryOrSPRepository
    {
        Task<Response<SpResult>> SpTestResult(string nombre, string apellido);

    }
}
