using System.Collections.Generic;
using System.Threading.Tasks;
using StocksApplication.Shared.Dtos;

namespace StocksApplication.Server.Services
{
    public interface ITickerService
    {
        public Task<IEnumerable<TickerDto>> GetTickersAsync(string search);
        public Task<CompanyDto> GetTickerDetailsAsync(int id);
        public Task<DailyOhlcDto> GetDailyOhlcAsync(int id);
        public Task<IEnumerable<OhclDto>> GetOhclsAsync(int id);
        public Task<IEnumerable<ArticleDto>> GetArticlesAsync(int id);
    }
}