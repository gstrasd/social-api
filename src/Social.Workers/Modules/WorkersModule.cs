using System;
using Autofac;
using System.Collections.Generic;
using Library.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Library.Dataflow;
using Library.Hosting;
using Library.Platform.Queuing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Social.Domain.Twitter;
using Social.Messages;
using Module = Autofac.Module;

namespace Social.Workers.Modules
{
    public class WorkersModule : Module
    {
        private readonly IConfiguration _configuration;

        public WorkersModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGenericQueueMessageWorkerFactory();

            builder.Register(c => c.Resolve<QueueMessageWorker<DiscoverInstagramAccountMessage>>(
                new NamedParameter("queue", "discover-instagram-account"),
                new NamedParameter("name", "Instagram Account Processor")))
                .SingleInstance()
                .As<IHostedService>();

            builder.Register(c => c.Resolve<QueueMessageWorker<DiscoverTwitterAccountMessage>>(
                    new NamedParameter("queue", "discover-twitter-account"),
                    new NamedParameter("name", "Twitter Account Processor")))
                .SingleInstance()
                .As<IHostedService>();

            builder.Register(c =>
                {
                    var buffer = c.Resolve<ISourceBlock<DiscoverInstagramAccountMessage>>();
                    return new DiscoverInstagramAccountMessageConsumer(buffer);
                })
                .SingleInstance()
                .As<MessageConsumer<DiscoverInstagramAccountMessage>>();

            builder.Register(c =>
                {
                    var buffer = c.Resolve<ISourceBlock<DiscoverTwitterAccountMessage>>();
                    return new DiscoverTwitterAccountMessageConsumer(c.Resolve<ITwitterService>(), buffer, c.Resolve<ILogger>());
                })
                .SingleInstance()
                .As<MessageConsumer<DiscoverTwitterAccountMessage>>();
        }
    }
}
