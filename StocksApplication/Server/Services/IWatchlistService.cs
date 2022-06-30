using System.Threading.Tasks;

namespace StocksApplication.Server.Services
{
    public interface IWatchlistService
    {
        public Task AddCompanyToWatchlistAsync(string userId, int companyId);
    }
}