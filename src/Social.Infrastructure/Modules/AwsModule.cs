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
using Autofac.Core;
using Library.Autofac;
using Library.Amazon;
using Library.Dataflow;
using Library.Messages;
using Library.Platform.Queuing;
using Microsoft.Extensions.Configuration;

namespace Social.Infrastructure.Modules
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
            builder.Register(_ =>
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
                    var options = c.ResolveNamed<AWSOptions>(_configuration["EnvironmentName"] ?? _configuration["ASPNETCORE_ENVIRONMENT"]);
                    var client = options.CreateServiceClient<IAmazonSQS>();

                    return client;
                })
                .OnlyIf(_ => _configuration["Providers:Queueing"].Equals("aws", StringComparison.InvariantCultureIgnoreCase))
                .InstancePerDependency()
                .As<IAmazonSQS>();

            builder.Register((c, p) =>
                {
                    var queue = p.TryGetValue("queue", out var value) ? value as string : null;
                    if (queue == null) throw new ArgumentNullException("queue", "To resolve an SQS queue client, the queue name must be supplied as a named parameter with the name \"queue\"..");

                    var client = c.Resolve<IAmazonSQS>();
                    var configuration = _configuration.GetSqsQueueClientConfiguration($"Aws:Queues:{queue}");
                    var queueClient = new SqsQueueClient(client, configuration);

                    return queueClient;
                })
                .OnlyIf(_ => _configuration["Providers:Queueing"].Equals("aws", StringComparison.InvariantCultureIgnoreCase))
                .InstancePerDependency()
                .As<IQueueClient>();
        }
    }
}
