using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Twitter
{
    public class TwitterUser
    {
        public string TwitterId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Username { get; set; } = default!;
        public DateTime? Created { get; set; }
        public string? Description { get; set; }
        public Uri? ProfileUrl { get; set; }
        public Uri? ProfileImageUrl { get; set; }
    }
}
