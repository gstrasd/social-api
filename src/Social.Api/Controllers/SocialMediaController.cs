using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Social.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class SocialMediaController : ControllerBase
    {
        private readonly ILogger _logger;

        public SocialMediaController(ILogger logger)
        {
            _logger = logger;
        }
    }
}