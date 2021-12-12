using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace Social.Infrastructure.Domain
{
    [DynamoDBTable("SearchHistory")]
    public class Search
    {
        [DynamoDBHashKey]
        [Required]
        public string Value { get; set; } = default!;
        public List<SearchResult> Results { get; set; } = new();
    }

    public class SearchResult
    {
        public SocialMediaType SocialMediaType { get; set; } = default!;
        public bool Success { get; set; } = false;
        public string? ProviderId { get; set; }
    }

    public enum SocialMediaType
    {
        Instagram,
        Twitter
    }
}
