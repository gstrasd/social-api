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
        Task<Search?> FindPreviousSearchAsync(string search, CancellationToken token = default);
        Task SaveSearchAsync(Search search, CancellationToken token = default);
    }
}
