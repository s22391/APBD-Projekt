using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StocksApplication.Server.Services;

namespace StocksApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _service;

        public WatchlistController(IWatchlistService service)
        {
            _service = service;
        }

        [HttpPost("{companyId}")]
        public async Task<IActionResult> AddCompanyToWatchlistAsync([FromRoute] int companyId)
        {
            await _service.AddCompanyToWatchlistAsync(GetUserId(), companyId);
            return Ok();
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}