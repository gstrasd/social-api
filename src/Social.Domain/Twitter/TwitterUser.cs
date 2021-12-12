using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Twitter
{
    public class TwitterUser
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Username { get; set; } = default!;
        public DateTime? Created { get; set; }
        public string? Description { get; set; }
        public Uri? ProfileUrl { get; set; }
        public Uri? ProfileImageUrl { get; set; }
    }

    public class Tweet
    {
        public string TweetId { get; set; } = default!;
        public string Text { get; set; } = String.Empty;
    }
}
