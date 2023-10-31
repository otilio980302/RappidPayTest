using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.DTOs.ViewModel;
using RappidPayTest.Application.Interfaces.Repositories;
using RappidPayTest.Application.Wrappers;

namespace RappidPayTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DapperTestController : ControllerBase
    {
        private readonly IDirectQueryOrSPRepository _directQueryOrSPRepository;
        public DapperTestController(IDirectQueryOrSPRepository directQueryOrSPRepository)
        {
            _directQueryOrSPRepository = directQueryOrSPRepository;
        }

        [HttpGet("SpTestResult")]
        [ProducesResponseType(typeof(Response<SpResult>), 200)]
        public async Task<IActionResult> SpTestResult(string nombre, string apellido)
        {
            return Ok(await _directQueryOrSPRepository.SpTestResult(nombre, apellido));
        }



    }
}
