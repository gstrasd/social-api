using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Infrastructure.Twitter
{
    public class TwitterConfiguration
    {
        public string ConsumerKey { get; set; } = default!;
        public string ConsumerSecret { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
        public string TokenSecret { get; set; } = default!;
        public string BearerToken { get; set; } = default!;
        public string BaseUrl { get; set; } = default!;
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromMinutes(1);
    }
}