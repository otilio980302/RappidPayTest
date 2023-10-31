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
    public class CardManagementController : OwnBaseController
    {
        private readonly ICardManagementService _cardManagementService;
        public CardManagementController(ICardManagementService cardManagementService)
        {
            _cardManagementService = cardManagementService;
        }
        //[Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<CardManagementVm>), 200)]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _cardManagementService.GetByIdAsync(id));
        }

        //[Authorize]
        [HttpGet("")]
        [ProducesResponseType(typeof(PagedResponse<IList<CardManagementVm>>), 200)]
        public async Task<IActionResult> GetAsync(int pageNumber, int pageSize, string filter = "")
        {
            var user = base.GetLoggerUser();
            var userId = base.GetLoggerUserId();



            return Ok(await _cardManagementService.GetPagedListAsync(pageNumber, pageSize, filter));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<IList<CardManagementVm>>), 200)]
        public async Task<IActionResult> PostAsync([FromBody] CardManagementDto obj)
        {
            return Ok(await _cardManagementService.InsertAsync(obj));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Response<IList<CardManagementVm>>), 200)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] CardManagementDto obj)
        {
            return Ok(await _cardManagementService.UpdateAsync(id, obj));
        }

    }
}
