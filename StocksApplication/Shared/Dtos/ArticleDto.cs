using System;

namespace StocksApplication.Shared.Dtos
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string Url { get; set; }
    }
}