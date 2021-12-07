using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Library.Dataflow;
using Serilog;
using Social.Messages;

namespace Social.Workers.Consumers
{
    internal class DiscoverInstagramAccountMessageConsumer : MessageConsumer<DiscoverInstagramAccountMessage>
    {
        private readonly ILogger _logger;

        public DiscoverInstagramAccountMessageConsumer(ISourceBlock<DiscoverInstagramAccountMessage> buffer, ILogger logger, CancellationToken token = default) : base(buffer, token)
        {
            _logger = logger;
        }

        protected override Task ConsumeMessageAsync(DiscoverInstagramAccountMessage message, CancellationToken token = default)
        {
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Task.CompletedTask;
        }
    }
}
