using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.DTOs.ViewModel;
using RappidPayTest.Application.Interfaces.Services;
using RappidPayTest.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using RappidPayTest.Api.Controllers.Base;

namespace RappidPayTest.Api.Controllers
{


    [Route("api/[controller]")]
    public class PrioridadesController : OwnBaseController
    {
        private readonly IPrioridadesService _prioridadesService;
        public PrioridadesController(IPrioridadesService prioridadesService)
        {
            _prioridadesService = prioridadesService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<PrioridadesVm>), 200)]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _prioridadesService.GetByIdAsync(id));
        }

        [Authorize]
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResponse<IList<PrioridadesVm>>), 200)]
        public async Task<IActionResult> GetAsync(int pageNumber, int pageSize, string filter = "")
        {
            var user = base.GetLoggerUser();
            var userId = base.GetLoggerUserId();



            return Ok(await _prioridadesService.GetPagedListAsync(pageNumber, pageSize, filter));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<IList<PrioridadesVm>>), 200)]
        public async Task<IActionResult> PostAsync([FromBody] PrioridadesDto obj)
        {
            return Ok(await _prioridadesService.InsertAsync(obj));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response<IList<PrioridadesVm>>), 200)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] PrioridadesDto obj)
        {
            return Ok(await _prioridadesService.UpdateAsync(id, obj));
        }

    }
}
