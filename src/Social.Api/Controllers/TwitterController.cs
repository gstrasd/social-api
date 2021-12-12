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
    [Route("twitter")]
    public class TwitterController : ControllerBase
    {
        private readonly ILogger _logger;

        public TwitterController(ILogger logger)
        {
            _logger = logger;
        }
    }
}