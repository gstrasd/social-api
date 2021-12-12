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
}
