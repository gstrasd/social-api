using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Social.Infrastructure.Domain
{
    public interface ISocialMediaRepository
    {
        Task<SearchResult?> FindSearchResultAsync(string value, SocialMediaType type, CancellationToken token = default);
        Task SaveSearchResultAsync(SearchResult searchResult, CancellationToken token = default);
    }
}
