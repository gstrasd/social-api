using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Social.Domain.Twitter;

namespace Social.Infrastructure.Domain
{
    [DynamoDBTable("SearchHistory")]
    public class SearchResult
    {
        [DynamoDBHashKey]
        public string Value { get; set; } = default!;
        [DynamoDBRangeKey]
        public SocialMediaType Type { get; set; }
        public bool Success { get; set; }
    }

    public enum SocialMediaType
    {
        Instagram = 0,
        Twitter
    }

    [DynamoDBTable("SocialProfile")]
    public class SocialProfile
    {
        public string ProviderId { get; set; } = default!;
        public string? TwitterId { get; set; }
        public string? InstagramId { get; set; }
        public TwitterProfile? Twitter { get; set; }
    }

    public class TwitterProfile
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Username { get; set; } = default!;
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public string ProfileUrl { get; set; } = default!;
        public string? ProfileImageUrl { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
