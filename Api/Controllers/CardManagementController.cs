﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Interfaces.Services;
using RapidPayTest.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using RapidPayTest.Api.Controllers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RapidPayTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class CardManagementController : OwnBaseController
    {
        private readonly ICardManagementService _cardManagementService;
        public CardManagementController(ICardManagementService cardManagementService)
        {
            _cardManagementService = cardManagementService;
        }

        [HttpGet("GetAccountBalance")]
        [ProducesResponseType(typeof(Response<CardManagementVm>), 200)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _cardManagementService.GetAccountBalance());
        }

        [HttpGet("GetMyTransactions")]
        [ProducesResponseType(typeof(PagedResponse<IList<TransactionVm>>), 200)]
        public async Task<IActionResult> GetAsync(int pageNumber, int pageSize, string filter = "")
        {
            return Ok(await _cardManagementService.GetTransactionPagedListAsync(pageNumber, pageSize, filter));
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost("CreateAccount")]
        [ProducesResponseType(typeof(Response<IList<CardManagementVm>>), 200)]
        public async Task<IActionResult> PostAsync([FromBody] CardManagementCreateDto obj)
        {
            return Ok(await _cardManagementService.InsertAsync(obj));
        }

        [HttpPut("UpdateAccountBalance")]
        [ProducesResponseType(typeof(Response<IList<CardManagementVm>>), 200)]
        public async Task<IActionResult> PutAsync(CardManagmentUpdateBalanceDto obj)
        {
            return Ok(await _cardManagementService.UpdateAsync(obj));
        }

    }
}
