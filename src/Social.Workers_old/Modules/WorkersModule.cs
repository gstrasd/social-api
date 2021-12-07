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
            /* Instagram Account Processor */
            builder.Register(c => new QueueMessageProducer<DiscoverInstagramAccountMessage>(
                    c.Resolve<IQueueClient>(new NamedParameter("queue", "discover-instagram-account")),
                    c.Resolve<ITargetBlock<DiscoverInstagramAccountMessage>>()
                ))
                .SingleInstance()
                .As<MessageProducer<DiscoverInstagramAccountMessage>>();

            builder.Register(c => new DiscoverInstagramAccountMessageConsumer(
                    c.Resolve<ISourceBlock<DiscoverInstagramAccountMessage>>(), 
                    c.Resolve<ILogger>()))
                .SingleInstance()
                .As<MessageConsumer<DiscoverInstagramAccountMessage>>();

            /* Twitter Account Processor */
            builder.Register(c => new QueueMessageProducer<DiscoverTwitterAccountMessage>(
                    c.Resolve<IQueueClient>(new NamedParameter("queue", "discover-twitter-account")),
                    c.Resolve<ITargetBlock<DiscoverTwitterAccountMessage>>()
                ))
                .SingleInstance()
                .As<MessageProducer<DiscoverTwitterAccountMessage>>();

            builder.Register(c => new DiscoverTwitterAccountMessageConsumer(
                    c.Resolve<ITwitterService>(), 
                    c.Resolve<IQueueClient>(new NamedParameter("queue", "reconcile-tweets")),
                    c.Resolve<ISourceBlock<DiscoverTwitterAccountMessage>>(), 
                    c.Resolve<ILogger>()))
                .SingleInstance()
                .As<MessageConsumer<DiscoverTwitterAccountMessage>>();

            /* Twitter Tweet Processor */
            builder.Register(c => new QueueMessageProducer<ReconcileTweetsMessage>(
                    c.Resolve<IQueueClient>(new NamedParameter("queue", "reconcile-tweets")),
                    c.Resolve<ITargetBlock<ReconcileTweetsMessage>>()
                ))
                .SingleInstance()
                .As<MessageProducer<ReconcileTweetsMessage>>();

            builder.Register(c => new ReconcileTweetsMessageConsumer(
                    c.Resolve<ITwitterService>(), 
                    c.Resolve<ISourceBlock<ReconcileTweetsMessage>>(), 
                    c.Resolve<ILogger>()))
                .SingleInstance()
                .As<MessageConsumer<ReconcileTweetsMessage>>();
        }
    }
}
