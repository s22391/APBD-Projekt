using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApplication.Server.Models;
using StocksApplication.Server.Services;
using StocksApplication.Shared.Dtos;

namespace StocksApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TickerController : ControllerBase
    {
        private readonly ITickerService _service;

        public TickerController(ITickerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickersAsync([FromQuery] string search)
        {
            search ??= "a";
            return Ok(await _service.GetTickersAsync(search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTickerDetailsAsync([FromRoute] int id)
        {
            return Ok(new CompanyDataDto
            {
                Company = await _service.GetTickerDetailsAsync(id),
                DailyOhlc = await _service.GetDailyOhlcAsync(id),
                Ohlcs = await _service.GetOhclsAsync(id),
                Articles = await _service.GetArticlesAsync(id)
            });
        }
    }
}