using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StocksApplication.Server.Data;
using StocksApplication.Server.Dtos;
using StocksApplication.Server.Models;
using StocksApplication.Shared.Dtos;

namespace StocksApplication.Server.Services
{
    public class TickerService : ITickerService
    {
        private readonly ApplicationDbContext _context;
        
        private readonly HttpClient _httpClient;

        private const string PolygonApiKey = "poOXr4lNviwpobp_nIuCxPp2EamJgdMO";

        public TickerService(ApplicationDbContext context, IHttpClientFactory factory)
        {
            _context = context;
            _httpClient = factory.CreateClient("polygonClient");
        }

        public async Task<IEnumerable<TickerDto>> GetTickersAsync(string search)
        {
            var tickersFromDb = _context.Companies.Where(e => e.Ticker.ToUpper().StartsWith(search.ToUpper()))
                .OrderBy(e => e.Ticker).Take(20);
            if (tickersFromDb.Any())
            {
                return tickersFromDb.Select(e => new TickerDto
                {
                    Id = e.Id,
                    Ticker = e.Ticker,
                    Name = e.Name
                }).ToList();
            }
            try
            {
                var response = await _httpClient.GetFromJsonAsync<WrapperDto<IEnumerable<TickerDto>>>(
                    $"v3/reference/tickers?market=stocks&limit=20&search={search}&apiKey={PolygonApiKey}");

                if (response is null)
                {
                    return null;
                }
                var companies = response.Results.Where(e => e.Ticker.ToUpper().StartsWith(search.ToUpper())).Select(e => new Company
                {
                    Ticker = e.Ticker,
                    Name = e.Name,
                    HasData = false
                }).ToList();
                await _context.AddRangeAsync(companies);
                await _context.SaveChangesAsync();
                return companies.Select(e => new TickerDto
                {
                    Id = e.Id,
                    Ticker = e.Ticker,
                    Name = e.Name
                }).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CompanyDto> GetTickerDetailsAsync(int id)
        {
            var tickerFromDb = await _context.Companies.SingleAsync(e => e.Id == id);
            if (!tickerFromDb.HasData)
            {
                try
                {
                    var response =
                        await _httpClient.GetFromJsonAsync<WrapperDto<TickerDetailsDto>>(
                            $"v3/reference/tickers/{tickerFromDb.Ticker}?apiKey={PolygonApiKey}");
                    if (response is null)
                    {
                        return null;
                    }

                    var tickerFromPolygon = response.Results;
                    tickerFromDb.Locale = tickerFromPolygon.Locale;
                    tickerFromDb.SicDescription = tickerFromPolygon.SicDescription;
                    tickerFromDb.HomePageUrl = tickerFromPolygon.HomePageUrl;
                    if (tickerFromPolygon.Branding is not null)
                    {
                        tickerFromDb.LogoUrl = $"{tickerFromPolygon.Branding.LogoUrl}?apiKey={PolygonApiKey}";
                    }
                    tickerFromDb.HasData = true;
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            
            return new CompanyDto
            {
                Id = tickerFromDb.Id,
                Ticker = tickerFromDb.Ticker,
                Name = tickerFromDb.Name,
                Locale = tickerFromDb.Locale,
                SicDescription = tickerFromDb.SicDescription,
                HomePageUrl = tickerFromDb.HomePageUrl,
                LogoUrl = tickerFromDb.LogoUrl
            };
        }

        public async Task<DailyOhlcDto> GetDailyOhlcAsync(int id)
        {
            var ohlcFromDb = await _context.DailyOhlcs.SingleOrDefaultAsync(e => e.Id == id);
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone).AddDays(-1).Date;
            if (ohlcFromDb is not null && ohlcFromDb.From == easternTime)
            {
                return new DailyOhlcDto
                {
                    Id = ohlcFromDb.Id,
                    From = ohlcFromDb.From,
                    Symbol = ohlcFromDb.Symbol,
                    Open = ohlcFromDb.Open,
                    High = ohlcFromDb.High,
                    Low = ohlcFromDb.Low,
                    Close = ohlcFromDb.Close,
                    Volume = ohlcFromDb.Volume,
                    AfterHours = ohlcFromDb.AfterHours,
                    PreMarket = ohlcFromDb.PreMarket
                };
            }

            try
            {
                var company = _context.Companies.Single(e => e.Id == id);
                var response = await _httpClient.GetFromJsonAsync<DailyOhlcDto>(
                    $"v1/open-close/{company.Ticker}/{easternTime:yyyy-MM-dd}?adjusted=true&apiKey={PolygonApiKey}");
                if (response is null)
                {
                    return null;
                }

                if (ohlcFromDb is not null)
                {
                    ohlcFromDb.From = DateTime.Today.AddDays(-1);
                    ohlcFromDb.Open = response.Open;
                    ohlcFromDb.High = response.High;
                    ohlcFromDb.Low = response.Low;
                    ohlcFromDb.Close = response.Close;
                    ohlcFromDb.Volume = response.Volume;
                    ohlcFromDb.AfterHours = response.AfterHours;
                    ohlcFromDb.PreMarket = response.PreMarket;
                }
                else
                {
                    await _context.DailyOhlcs.AddAsync(new DailyOhlc
                    {
                        Id = id,
                        From = easternTime,
                        Symbol = response.Symbol,
                        Open = response.Open,
                        High = response.High,
                        Low = response.Low,
                        Close = response.Close,
                        Volume = response.Volume,
                        AfterHours = response.AfterHours,
                        PreMarket = response.PreMarket
                    });
                }

                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<OhclDto>> GetOhclsAsync(int id)
        {
            var ticker = (await _context.Companies.SingleAsync(e => e.Id == id)).Ticker;
            try
            {
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
                var response = await _httpClient.GetFromJsonAsync<WrapperDto<IEnumerable<OhclDto>>>(
                    $"v2/aggs/ticker/{ticker}/range/1/day/{easternTime.AddMonths(-3):yyyy-MM-dd}/{easternTime:yyyy-MM-dd}?adjusted=true&sort=asc&limit=200&apiKey={PolygonApiKey}");
                return response?.Results;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync(int id)
        {
            var ticker = (await _context.Companies.SingleAsync(e => e.Id == id)).Ticker;
            try
            {
                var response =
                    await _httpClient.GetFromJsonAsync<WrapperDto<IEnumerable<NewsDto>>>(
                        $"v2/reference/news?ticker={ticker}&limit=5&apiKey={PolygonApiKey}");

                return response?.Results.Select(e => new ArticleDto
                {
                    Title = e.Title,
                    Author = e.Author,
                    Published = e.PublishedUtc.ToLocalTime(),
                    Url = e.Url
                });
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}