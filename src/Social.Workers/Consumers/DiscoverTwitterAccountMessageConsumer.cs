using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Library.Dataflow;
using Library.Platform.Queuing;
using Serilog;
using Social.Domain.Twitter;
using Social.Infrastructure;
using Social.Infrastructure.Domain;
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
                /*
                 *  1.  Look up provider to see if this Twitter account already exists
                 *  2.  If not found, look to see if this potential Twitter username has been checked before
                 *  3.  If found, try to find the Twitter id for username
                 *  4.  If not found, add a db record to indicate that this username look up failed so it won't be tried again
                 *  5.  If found, add a db record to link this Twitter account to the provider
                 *  6.  Send a reconcile-tweets message
                 */





                //_logger.Verbose(JsonSerializer.Serialize(message, new JsonSerializerOptions { WriteIndented = true }));
                var user = await _service.GetUserByUsernameAsync(message.TwitterUsername, token);
                if (user == null)
                {
                    _logger.Information($"Twitter user {message.TwitterUsername} does not exist.");
                    return;
                }

                //_logger.Verbose(JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }));
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
                _logger.Error(e, $"An error occurred while trying to process message \"{message.CorrelationId}\" of type \"{message.GetType()}\". The message will be forwarded to the dead letter queue.");
            }
        }
    }
}