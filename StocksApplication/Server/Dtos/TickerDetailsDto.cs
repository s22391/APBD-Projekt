using System.Text.Json.Serialization;

namespace StocksApplication.Shared.Dtos
{
    public class TickerDetailsDto
    {
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        [JsonPropertyName("sic_description")]
        public string SicDescription { get; set; }
        [JsonPropertyName("homepage_url")]
        public string HomePageUrl { get; set; }
        public BrandingDto Branding { get; set; }
    }

    public class BrandingDto
    {
        [JsonPropertyName("logo_url")]
        public string LogoUrl { get; set; }
    }
}