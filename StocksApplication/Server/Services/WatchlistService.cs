using System.Threading.Tasks;
using StocksApplication.Server.Data;

namespace StocksApplication.Server.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly ApplicationDbContext _context;

        public WatchlistService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task AddCompanyToWatchlistAsync(string userId, int companyId)
        {
            
        }
    }
}