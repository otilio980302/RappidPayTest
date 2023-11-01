using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.Settings;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Interfaces.Services;
using RapidPayTest.Application.Interfaces.Services.Security;
using RapidPayTest.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using RapidPayTest.Infrastructure.Services;

namespace RapidPayTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountControllerController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountControllerController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [Authorize(Policy = "RequireAdmin")]
        [HttpPost("Register")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> RegisterAsync([FromBody] UserDto obj)
        {
            return Ok(await _accountService.RegisterAsync(obj, ""));
        }

        [AllowAnonymous]
        [HttpPost("LogIn")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> PostAsync([FromBody] AuthenticationRequest obj)
        {
            return Ok(await _accountService.AuthenticateAsync(obj, GenerateIPAddress()));
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(PagedResponse<IList<UserVm>>), 200)]
        public async Task<IActionResult> GetAsync(int pageNumber, int pageSize, string filter = "")
        {

            return Ok(await _accountService.GetUsers(pageNumber, pageSize, filter));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
