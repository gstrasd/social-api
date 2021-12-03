using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Social.Domain;

namespace Social.Api.Controllers
{
    [ApiController]
    [Route("instagram")]
    public class InstagramController : ControllerBase
    {
        private readonly IInstagramService _service;
        private readonly ILogger _logger;

        public InstagramController(IInstagramService service, ILogger logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("oembed")]
        public Task<string> GetOEmbedAsync(Uri postUrl)
        {
            return _service.GetPostHtmlAsync(postUrl);
        }
    }
}
