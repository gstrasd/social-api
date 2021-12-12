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
using Social.Domain.Twitter;
using Social.Messages;

namespace Social.Workers.Consumers
{
    internal class ReconcileTweetsMessageConsumer : MessageConsumer<ReconcileTweetsMessage>
    {
        private readonly ITwitterService _service;
        private readonly ILogger _logger;

        public ReconcileTweetsMessageConsumer(ITwitterService service, ISourceBlock<ReconcileTweetsMessage> buffer, ILogger logger, CancellationToken token = default) : base(buffer, token)
        {
            _service = service;
            _logger = logger;
        }

        protected override async Task ConsumeMessageAsync(ReconcileTweetsMessage message, CancellationToken token = default)
        {
            try
            {
                _logger.Verbose(JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true }));
                var tweets = await _service.GetTweetsByUserId(message.TwitterUserId, default, default, 50, token);

                _logger.Verbose(JsonSerializer.Serialize(tweets, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"An error occurred while trying to process message \"{message.CorrelationId}\" of type \"{message.GetType()}\". The message will be forwarded to the dead letter queue.");
            }
        }
    }
}