using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain
{
    public class OEmbed
    {
        public OEmbedType Type { get; set; } = default!;
        public Version Version { get; set; } = default!;
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public Uri? AuthorUrl { get; set; }
        public string? ProviderName { get; set; }
        public Uri? ProviderUrl { get; set; }
        public int? CacheAge { get; set; }
        public Uri? ThumbnailUrl { get; set; }
        public int? ThumbnailWidth { get; set; }
        public int? ThumbnailHeight { get; set; }
        public string? Html { get; set; }
    }

    public enum OEmbedType
    {
        Photo,
        Video,
        Link,
        Rich
    }
}
