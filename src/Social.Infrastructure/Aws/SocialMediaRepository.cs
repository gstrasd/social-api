using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Serilog;
using Social.Infrastructure.Domain;

namespace Social.Infrastructure.Aws
{
    public class SocialMediaRepository : ISocialMediaRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly ILogger _logger;

        public SocialMediaRepository(IDynamoDBContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<SearchResult?> FindSearchResultAsync(string value, SocialMediaType type, CancellationToken token = default)
        {
            return _context.LoadAsync<SearchResult?>(value, (int) type, token);
        }

        public Task SaveSearchResultAsync(SearchResult searchResult, CancellationToken token = default)
        {
            return _context.SaveAsync(searchResult, token);
        }

        public Task<SocialProfile> FindProfileByProviderIdAsync(string providerId, CancellationToken token = default)
        {
            return _context.LoadAsync<SocialProfile>(providerId, token);
        }

        public async Task<SocialProfile?> FindProfileByTwitterIdAsync(string twitterId, CancellationToken token = default)
        {
            var request = new QueryRequest
            {
                TableName = "SocialProfile",
                IndexName = "ByTwitterId",
                KeyConditionExpression = $"TwitterId = {twitterId}"
            };
            var query = _context.QueryAsync<SocialProfile>(request);
            var result = await query.GetNextSetAsync(token);
            return result.FirstOrDefault();
        }

        public async Task<SocialProfile?> FindProfileByTwitterUsernameAsync(string twitterUsername, CancellationToken token = default)
        {
            var request = new QueryRequest
            {
                TableName = "SocialProfile",
                IndexName = "ByTwitterUsername",
                KeyConditionExpression = $"InstagramUsername = {twitterUsername}"
            };
            var query = _context.QueryAsync<SocialProfile>(request);
            var result = await query.GetNextSetAsync(token);
            return result.FirstOrDefault();
        }
    }
}
