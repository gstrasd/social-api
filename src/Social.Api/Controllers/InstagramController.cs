using System;
using System.Threading.Tasks;
using System.Web;
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

        [HttpGet("oembed/{url}")]
        public async Task<IActionResult> GetOEmbedAsync(string url)
        {
            var decodedUrl = HttpUtility.UrlDecode(url);
            if (!Uri.TryCreate(decodedUrl, UriKind.Absolute, out var postUrl))
            {
                return BadRequest(new { });
            }

            try
            {
                var html = await _service.GetPostHtmlAsync(postUrl!).ConfigureAwait(false);
                return Ok(new { html });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}
