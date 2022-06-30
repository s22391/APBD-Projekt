using System;

namespace StocksApplication.Shared.Dtos
{
    public class DailyOhlcDto
    {
        public int Id { get; set; } 
        public DateTime From { get; set; } 
        public string Symbol { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public double AfterHours { get; set; }
        public double PreMarket { get; set; }
    }
}