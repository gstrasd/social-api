using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using Autofac;
using Library.Amazon;
using Library.Dataflow;
using Library.Messages;
using Library.Platform.Queuing;
using Microsoft.Extensions.Configuration;

namespace Social.Workers.Modules
{
    public class AwsModule : Module
    {
        private readonly IConfiguration _configuration;

        public AwsModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var accessKey = _configuration["Aws:AccessKey"];
                    var secretKey = _configuration["Aws:SecretKey"];
                    var options = _configuration.GetAWSOptions("Aws");
                    options.Credentials = new BasicAWSCredentials(accessKey, secretKey);
                    return options;
                })
                .Named<AWSOptions>("Development")
                .SingleInstance();

            builder.Register(c =>
                {
                    var options = c.ResolveNamed<AWSOptions>(_configuration["EnvironmentName"]);
                    var client = options.CreateServiceClient<IAmazonSQS>();
                    return client;
                })
                .As<IAmazonSQS>()
                .InstancePerDependency();

            builder.Register(c =>
                {
                    var configuration = _configuration.GetSqsQueueClientConfiguration("Aws:Queues:ProcessInstagramAccountMessage");
                    var client = new SqsQueueClient(c.Resolve<IAmazonSQS>(), configuration);
                    var buffer = c.Resolve<ITargetBlock<ProcessInstagramAccountMessage>>();
                    var producer = new QueueMessageProducer<ProcessInstagramAccountMessage>(client, buffer);

                    return producer;
                })
                .InstancePerDependency()
                .As<MessageProducer<ProcessInstagramAccountMessage>>();
        }
    }
}
