using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using StocksApplication.Shared.Dtos;

namespace StocksApplication.Server.Services
{
    public interface IWatchlistService
    {
        public Task<bool> AddCompanyToWatchlistAsync(string userId, int companyId);
        public Task<bool> IsCompanyInWatchlistAsync(string userId, int companyId);
        public Task<IEnumerable<CompanyDto>> GetWatchlistElementsAsync(string userId);
        public Task DeleteCompanyFromWatchlistAsync(string userId, int companyId);
    }
}