using System.Collections.Generic;

namespace StocksApplication.Shared.Dtos
{
    public class CompanyDataDto
    {
        public CompanyDto Company { get; set; }
        public DailyOhlcDto DailyOhlc { get; set; }
        public IEnumerable<OhclDto> Ohlcs { get; set; }
        public IEnumerable<ArticleDto> Articles { get; set; }
    }
}