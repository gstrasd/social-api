using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Social.Domain;
using Social.Domain.Instagram;

namespace Social.Application
{
    internal class CachedInstagramService : IInstagramService
    {
        private readonly IInstagramService _service;
        private readonly ILogger _logger;

        public CachedInstagramService(IInstagramService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        public Task<string> GetPostHtmlAsync(Uri postUrl)
        {
            return _service.GetPostHtmlAsync(postUrl);
        }
    }
}