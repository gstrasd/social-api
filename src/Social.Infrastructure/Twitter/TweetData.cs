using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Social.Infrastructure.Twitter
{
    internal class TweetData
    {
        [JsonPropertyName("id")]
        public string  Id { get; set; } = default!;
        [JsonPropertyName("text")]
        public string Text { get; set; } = String.Empty;
    }
}