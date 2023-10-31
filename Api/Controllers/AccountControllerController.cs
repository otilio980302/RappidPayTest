using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RappidPayTest.Application.DTOs;
using RappidPayTest.Application.DTOs.Settings;
using RappidPayTest.Application.DTOs.ViewModel;
using RappidPayTest.Application.Interfaces.Services;
using RappidPayTest.Application.Interfaces.Services.Security;
using RappidPayTest.Application.Wrappers;

namespace BaseBackendNet6.Api.Controllers
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



        [HttpPost("Register")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest obj)
        {
            return Ok(await _accountService.RegisterAsync(obj, ""));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> PostAsync([FromBody] AuthenticationRequest obj)
        {
            return Ok(await _accountService.AuthenticateAsync(obj, GenerateIPAddress()));
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
