using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
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
    }
}
