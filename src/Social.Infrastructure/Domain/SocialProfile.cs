using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace Social.Infrastructure.Domain
{
    [DynamoDBTable("SocialProfile")]
    public class SocialProfile
    {
        [DynamoDBHashKey]
        public string ProviderId { get; set; } = default!;
        [DynamoDBGlobalSecondaryIndexHashKey("ByTwitterId")]
        public string? TwitterId { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey("ByTwitterUsername")]
        public string? TwitterUsername { get; set; }
        public TwitterProfile? Twitter { get; set; }
    }
}