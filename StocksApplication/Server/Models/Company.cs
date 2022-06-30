using System.Collections.Generic;
using StocksApplication.Shared.Dtos;

namespace StocksApplication.Server.Models
{
    public class Company
    {
        public Company()
        {
            Users = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string SicDescription { get; set; }
        public string HomePageUrl { get; set; }
        public string LogoUrl { get; set; }
        public bool HasData { get; set; }
        
        public virtual DailyOhlc DailyOhlc { get; set; }
        public virtual  ICollection<ApplicationUser> Users { get; set; }
    }
}