namespace StocksApplication.Shared.Dtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string SicDescription { get; set; }
        public string HomePageUrl { get; set; }
        public string LogoUrl { get; set; }
    }
}