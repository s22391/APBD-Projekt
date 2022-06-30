using System;

namespace StocksApplication.Shared.Models
{
    public class OhlcModel
    {
        public double V { get; set; }
        public double O { get; set; }
        public double C { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public DateTime T { get; set; }
    }
}