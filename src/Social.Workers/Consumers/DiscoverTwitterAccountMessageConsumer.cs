using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Library.Dataflow;
using Library.Platform.Queuing;
using Serilog;
using Social.Domain.Twitter;
using Social.Messages;

namespace Social.Workers.Consumers
{
    internal class DiscoverTwitterAccountMessageConsumer : MessageConsumer<DiscoverTwitterAccountMessage>
    {
        private readonly ITwitterService _service;
        private readonly IQueueClient _queueClient;
        private readonly ILogger _logger;

        public DiscoverTwitterAccountMessageConsumer(ITwitterService service, IQueueClient queueClient, ISourceBlock<DiscoverTwitterAccountMessage> buffer, ILogger logger, CancellationToken token = default) :
            base(buffer, token)
        {
            _service = service;
            _queueClient = queueClient;
            _logger = logger;
        }

        protected override async Task ConsumeMessageAsync(DiscoverTwitterAccountMessage message, CancellationToken token = default)
        {
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true }));
                var user = await _service.GetUserByUsernameAsync(message.TwitterUsername, token);
                if (user == null)
                {
                    _logger.Information($"Twitter user {message.TwitterUsername} does not exist.");
                    return;
                }

                Console.WriteLine(JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }));
                var reconcileMessage = new ReconcileTweetsMessage
                {
                    CorrelationId = message.CorrelationId,
                    ProviderId = message.ProviderId,
                    TwitterUserId = user.UserId
                };
                _queueClient.WriteMessageAsync(reconcileMessage, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);   // TODO: Log error
            }
        }
    }
}