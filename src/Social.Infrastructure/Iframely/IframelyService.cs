using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Serilog;
using Social.Domain;

namespace Social.Infrastructure.Iframely
{
    internal class IframelyService : IOEmbedService
    {
        private readonly HttpClient _client;
        private readonly IframelyConfiguration _configuration;
        private readonly ILogger _logger;

        public IframelyService(HttpClient client, IframelyConfiguration configuration, ILogger logger)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<OEmbed> GetOEmbedAsync(Uri postUrl)
        {
            _logger.Information($"Getting oEmbed response for {postUrl}");

            var url = String.Format(_configuration.OEmbedUrl, new object[] { HttpUtility.UrlEncode(postUrl.ToString()), _configuration.ApiKey });
            var response = await _client.GetAsync(url).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var iframelyOEmbed = JsonSerializer.Deserialize<IframelyOEmbedResponse>(json);
            var oEmbed = new OEmbed
            {
                Type = Enum.Parse<OEmbedType>(iframelyOEmbed.Type, true),
                Version = Version.Parse(iframelyOEmbed.Version),
                Title = iframelyOEmbed.Title,
                AuthorName = iframelyOEmbed.Author,
                AuthorUrl = iframelyOEmbed.Url,
                ProviderName = iframelyOEmbed.ProviderName,
                ProviderUrl = null,
                CacheAge = iframelyOEmbed.CacheAge,
                ThumbnailUrl = iframelyOEmbed.ThumbnailUrl,
                ThumbnailWidth = iframelyOEmbed.ThumbnailWidth,
                ThumbnailHeight = iframelyOEmbed.ThumbnailHeight,
                Html = iframelyOEmbed.Html
            };

            return oEmbed;
        }
    }
}
