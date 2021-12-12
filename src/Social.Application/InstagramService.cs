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
    internal class InstagramService : IInstagramService
    {
        private readonly IOEmbedService _oEmbedService;
        private readonly ILogger _logger;

        public InstagramService(IOEmbedService oEmbedService, ILogger logger)
        {
            _oEmbedService = oEmbedService;
            _logger = logger;
        }

        public async Task<string> GetPostHtmlAsync(Uri postUrl)
        {
            var oEmbed = await _oEmbedService.GetOEmbedAsync(postUrl).ConfigureAwait(false);
            return oEmbed.Html;
        }
    }
}
