using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Social.Infrastructure.Iframely
{
    internal class IframelyOEmbedResponse
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("provider_name")]
        public string ProviderName { get; set; }
        [JsonPropertyName("thumbnail_url")]
        public Uri ThumbnailUrl { get; set; }
        [JsonPropertyName("thumbnail_width")]
        public int ThumbnailWidth { get; set; }
        [JsonPropertyName("thumbnail_height")]
        public int ThumbnailHeight { get; set; }
        [JsonPropertyName("html")]
        public string Html { get; set; }
        [JsonPropertyName("cache_age")]
        public int? CacheAge { get; set; }
    }
}
