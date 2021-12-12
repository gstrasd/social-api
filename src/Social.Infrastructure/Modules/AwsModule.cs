using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using Autofac;
using Autofac.Core;
using Library.Autofac;
using Library.Amazon;
using Library.Dataflow;
using Library.Platform.Queuing;
using Library.Platform.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Social.Infrastructure.Aws;
using Social.Infrastructure.Domain;

namespace Social.Infrastructure.Modules
{
    public class AwsModule : Module
    {
        private readonly HostBuilderContext _context;

        public AwsModule(HostBuilderContext context)
        {
            _context = context;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // AWS configuration
            builder.Register(_ =>
                {
                    var accessKey = _context.Configuration["Aws:AccessKey"];
                    var secretKey = _context.Configuration["Aws:SecretKey"];
                    var options = _context.Configuration.GetAWSOptions("Aws");
                    options.Credentials = new BasicAWSCredentials(accessKey, secretKey);
                    return options;
                })
                .Named<AWSOptions>("Development")
                .SingleInstance();

            // SQS client
            builder.Register(c =>
                {
                    var options = c.ResolveNamed<AWSOptions>(_context.HostingEnvironment.EnvironmentName);
                    var client = options.CreateServiceClient<IAmazonSQS>();

                    return client;
                })
                .InstancePerDependency()
                .As<IAmazonSQS>();

            // DynamoDB client
            builder.Register(c =>
                {
                    var options = c.ResolveNamed<AWSOptions>(_context.HostingEnvironment.EnvironmentName);
                    var client = options.CreateServiceClient<IAmazonDynamoDB>();
                    return client;
                })
                .InstancePerDependency()
                .As<IAmazonDynamoDB>();

            // DynamoDB context
            builder.Register(c => new DynamoDBContext(c.Resolve<IAmazonDynamoDB>()))
                .InstancePerDependency()
                .As<IDynamoDBContext>();

            /******************************************** AWS-based Infrastructural components ***********************************************/

            // Infrastructure queue client
            builder.Register((c, p) =>
                {
                    var queue = p.TryGetValue("queue", out var value) ? value as string : null;
                    if (queue == null) throw new ArgumentNullException("queue", "To resolve an SQS queue client, the queue name must be supplied as a named parameter with the name \"queue\"..");

                    var client = c.Resolve<IAmazonSQS>();
                    var configuration = _context.Configuration.GetSqsQueueClientConfiguration($"Aws:Queues:{queue}");
                    var queueClient = new SqsQueueClient(client, configuration);

                    return queueClient;
                })
                .OnlyIf(_ => _context.Configuration["PlatformProviders:Queueing"].Equals("aws", StringComparison.InvariantCultureIgnoreCase))
                .InstancePerDependency()
                .As<IQueueClient>();

            builder.Register(c => new SocialMediaRepository(c.Resolve<IDynamoDBContext>(), c.Resolve<ILogger>()))
                .OnlyIf(_ => _context.Configuration["PlatformProviders:Queueing"].Equals("aws", StringComparison.InvariantCultureIgnoreCase))
                .SingleInstance()
                .As<ISocialMediaRepository>();

          
        }
    }
}
