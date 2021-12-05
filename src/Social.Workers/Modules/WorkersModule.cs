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
using Library.Messages;
using Library.Messages.Social;
using Library.Platform.Queuing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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

            builder.Register(c => c.Resolve<QueueMessageWorker<ProcessInstagramAccountMessage>>(
                new NamedParameter("queue", "process-instagram-account"),
                new NamedParameter("name", "Instagram Account Processor")))
                .SingleInstance()
                .As<IHostedService>();

            builder.Register(c =>
                {
                    var buffer = c.Resolve<ISourceBlock<ProcessInstagramAccountMessage>>();
                    return new ProcessInstagramAccountMessageConsumer(buffer);
                })
                .SingleInstance()
                .As<MessageConsumer<ProcessInstagramAccountMessage>>();
        }
    }
}
