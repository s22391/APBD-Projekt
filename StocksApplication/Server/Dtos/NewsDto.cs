using System;
using System.Text.Json.Serialization;

namespace StocksApplication.Server.Dtos
{
    public class NewsDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        [JsonPropertyName("published_utc")]
        public DateTime PublishedUtc { get; set; }
        [JsonPropertyName("article_url")]
        public string Url { get; set; }
    }
}