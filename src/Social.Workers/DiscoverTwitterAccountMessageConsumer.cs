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

namespace Social.Workers
{
    internal class DiscoverTwitterAccountMessageConsumer : MessageConsumer<DiscoverTwitterAccountMessage>
    {
        private readonly ITwitterService _service;
        private readonly ILogger _logger;

        public DiscoverTwitterAccountMessageConsumer(ITwitterService service, ISourceBlock<DiscoverTwitterAccountMessage> buffer, ILogger logger, CancellationToken token = default) :
            base(buffer, token)
        {
            _service = service;
            _logger = logger;
        }

        protected override Task ConsumeMessageAsync(DiscoverTwitterAccountMessage message, CancellationToken token = default)
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

            //try
            //{
            //    var user = await _service.GetUserByUsernameAsync(message.TwitterUsername, token);
            //    Console.WriteLine(JsonSerializer.Serialize(user));

            //    if (user == null) return;


            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }
    }
}