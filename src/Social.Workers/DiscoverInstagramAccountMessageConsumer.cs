using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Library.Dataflow;
using Social.Messages;

namespace Social.Workers
{
    internal class DiscoverInstagramAccountMessageConsumer : MessageConsumer<DiscoverInstagramAccountMessage>
    {
        public DiscoverInstagramAccountMessageConsumer(ISourceBlock<DiscoverInstagramAccountMessage> buffer, CancellationToken token = default) : base(buffer, token)
        {
        }

        protected override Task ConsumeMessageAsync(DiscoverInstagramAccountMessage message, CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }
}
