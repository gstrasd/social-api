using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Infrastructure.Iframely
{
    internal class IframelyConfiguration
    {
        public string ApiKey { get; set; }
        public string ApiKeyHash { get; set; }
        public string OEmbedUrl { get; set; }
        public string IframelyUrl { get; set; }
    }
}
