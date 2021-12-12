using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.Platform.Storage;
using Serilog;
using Social.Infrastructure.Domain;

namespace Social.Infrastructure
{
    public class SocialMediaRepository : ISocialMediaRepository
    {
        private readonly ITableStorageClient _client;
        private readonly ILogger _logger;

        public SocialMediaRepository(ITableStorageClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task<Search?> FindPreviousSearchAsync(string search, CancellationToken token = default)
        {
            return _client.FindAsync<Search?>(new object[] {search}, token);
        }

        public Task SaveSearchAsync(Search search, CancellationToken token = default)
        {
            return _client.SaveAsync(search, token);
        }
    }
}
