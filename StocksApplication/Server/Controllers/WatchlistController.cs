using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StocksApplication.Server.Services;
using StocksApplication.Shared.Dtos;

namespace StocksApplication.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _service;

        public WatchlistController(IWatchlistService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyToWatchlistAsync([FromBody] CompanyIdDto dto)
        {
            Console.WriteLine(dto.CompanyId);
            if (await _service.AddCompanyToWatchlistAsync(GetUserId(), dto.CompanyId))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetWatchlistElementsAsync()
        {
            return Ok(await _service.GetWatchlistElementsAsync(GetUserId()));
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> IsCompanyInWatchlistAsync([FromRoute] int companyId)
        {
            return Ok(await _service.IsCompanyInWatchlistAsync(GetUserId(), companyId));
        }

        [HttpDelete("{companyId}")]
        public async Task<IActionResult> DeleteCompanyFromWatchlistAsync([FromRoute] int companyId)
        {
            await _service.DeleteCompanyFromWatchlistAsync(GetUserId(), companyId);
            return Ok();
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}