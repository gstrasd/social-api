using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Social.Infrastructure.Domain
{
    public interface ISocialMediaRepository : ISocialProfileRepository, ISearchRepository
    {
    }

    public interface ISocialProfileRepository
    {
        Task<SocialProfile> FindProfileByProviderIdAsync(string providerId, CancellationToken token = default);
        Task<SocialProfile?> FindProfileByTwitterIdAsync(string twitterId, CancellationToken token = default);
        Task<SocialProfile?> FindProfileByTwitterUsernameAsync(string twitterUsername, CancellationToken token = default);
    }

    public interface ISearchRepository
    {
        Task<SearchResult?> FindSearchResultAsync(string value, SocialMediaType type, CancellationToken token = default);
        Task SaveSearchResultAsync(SearchResult searchResult, CancellationToken token = default);
    }
}