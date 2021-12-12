using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Domain.Twitter;

namespace Social.Domain
{
    public class SocialMedia
    {
        public string ProviderId { get; set; } = default!;
        public TwitterUser? TwitterUser { get; set; }
    }
}
