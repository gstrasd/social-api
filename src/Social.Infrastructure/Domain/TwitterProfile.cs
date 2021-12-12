using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Infrastructure.Domain
{
    public class TwitterProfile
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Username { get; set; } = default!;
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public string ProfileUrl { get; set; } = default!;
        public string? ProfileImageUrl { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}