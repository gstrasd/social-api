using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Library.Dataflow;
using Library.Messages;
using Library.Messages.Social;

namespace Social.Workers
{
    internal class ProcessInstagramAccountMessageConsumer : MessageConsumer<ProcessInstagramAccountMessage>
    {
        public ProcessInstagramAccountMessageConsumer(ISourceBlock<ProcessInstagramAccountMessage> buffer, CancellationToken token = default) : base(buffer, token)
        {
        }

        protected override Task ConsumeMessageAsync(ProcessInstagramAccountMessage message, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
