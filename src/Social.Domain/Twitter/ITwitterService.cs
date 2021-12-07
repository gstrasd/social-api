using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Social.Domain.Twitter
{
    public interface ITwitterService
    {
        Task<TwitterUser?> GetUserByUsernameAsync(string username, CancellationToken token = default);
        Task<IEnumerable<Tweet>?> GetTweetsByUserId(string id, DateTime? startDate = default, DateTime? endDate = default, int maxCount = 50, CancellationToken token = default);
    }
}
