using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Amazon.SQS;
using Autofac;
using Library.Amazon;
using Library.Configuration;
using Library.Dataflow;
using Library.Hosting;
using Library.Messages;
using Library.Platform.Queuing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
            //builder.Register(c => new MessageWorker<ProcessInstagramAccountMessage>(
            //    c.Resolve<MessageProducer<ProcessInstagramAccountMessage>>(),
            //    c.Resolve<MessageConsumer<ProcessInstagramAccountMessage>>(),
            //    _configuration.Bind<MessageWorkerConfiguration>("Workers:ProcessInstagramAccount"))
            //    {
            //        Name = "Instagram Account Processor"
            //    })
            //    .SingleInstance()
            //    .As<IHostedService>();
        }
    }
}
