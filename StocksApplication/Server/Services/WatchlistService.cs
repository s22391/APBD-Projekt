using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StocksApplication.Server.Data;
using StocksApplication.Shared.Dtos;

namespace StocksApplication.Server.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDbContext _context;

        public WatchlistService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> AddCompanyToWatchlistAsync(string userId, int companyId)
        {
            var user = await _context.Users.Include(e => e.Companies).SingleAsync(e => e.Id == userId);
            if (user.Companies.Any(e => e.Id == companyId))
            {
                return false;
            }

            var company = await _context.Companies.SingleAsync(e => e.Id == companyId);
            user.Companies.Add(company);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CompanyDto>> GetWatchlistElementsAsync(string userId)
        {
            var user = await _context.Users.Include(e => e.Companies).SingleAsync(e => e.Id == userId);
            return user.Companies.Select(e => new CompanyDto
            {
                Id = e.Id,
                Ticker = e.Ticker,
                Name = e.Name,
                Locale = e.Locale,
                HomePageUrl = e.HomePageUrl,
                LogoUrl = e.LogoUrl,
                SicDescription = e.SicDescription
            });
        }

        public async Task<bool> IsCompanyInWatchlistAsync(string userId, int companyId)
        {
            var user = await _context.Users.Include(e => e.Companies).SingleAsync(e => e.Id == userId);
            return user.Companies.All(e => e.Id != companyId);
        }

        public async Task DeleteCompanyFromWatchlistAsync(string userId, int companyId)
        {
            var user = await _context.Users.Include(e => e.Companies).SingleAsync(e => e.Id == userId);
            var company = await _context.Companies.SingleAsync(e => e.Id == companyId);
            user.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}