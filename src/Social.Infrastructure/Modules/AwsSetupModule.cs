using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Autofac;
using Library.Amazon;
using Library.Installation;
using Library.Platform.Queuing;
using Microsoft.Extensions.Configuration;

namespace Social.Infrastructure.Modules
{
    public class AwsSetupModule : Module
    {
        private readonly IConfiguration _configuration;

        public AwsSetupModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // TODO: Only execute these steps if AWS is the queue provider
        protected override void Load(ContainerBuilder builder)
        {
            // Create discover-instagram-account queue
            builder.Register(c =>
                {
                    var manager = c.Resolve<IQueueManager>();
                    var step = new InstallerStep("Create discover-instagram-account queue", async () =>
                    {
                        var exists = await manager.QueueExistsAsync("discover-instagram-account");
                        if (exists)
                        {
                            Console.WriteLine("Queue already exists.");
                            return;
                        }
                        var queueUrl = await manager.CreateQueueAsync("discover-instagram-account");
                        Console.WriteLine($"Queue created with URL {queueUrl}.");
                    });
                    return step;
                })
                .SingleInstance()
                .As<IInstallerStep>();

            // Create discover-twitter-account queue
            builder.Register(c =>
                {
                    var manager = c.Resolve<IQueueManager>();
                    var step = new InstallerStep("Create discover-twitter-account queue", async () =>
                    {
                        var exists = await manager.QueueExistsAsync("discover-twitter-account");
                        if (exists)
                        {
                            Console.WriteLine("Queue already exists.");
                            return;
                        }
                        var queueUrl = await manager.CreateQueueAsync("discover-twitter-account");
                        Console.WriteLine($"Queue created with URL {queueUrl}.");
                    });
                    return step;
                })
                .SingleInstance()
                .As<IInstallerStep>();

            builder.Register(c => new SqsQueueManager(c.Resolve<IAmazonSQS>()))
                .As<IQueueManager>()
                .InstancePerDependency();

            // Create reconcile-tweets queue
            builder.Register(c =>
                {
                    var manager = c.Resolve<IQueueManager>();
                    var step = new InstallerStep("Create reconcile-tweets queue", async () =>
                    {
                        var exists = await manager.QueueExistsAsync("reconcile-tweets");
                        if (exists)
                        {
                            Console.WriteLine("Queue already exists.");
                            return;
                        }
                        var queueUrl = await manager.CreateQueueAsync("reconcile-tweets");
                        Console.WriteLine($"Queue created with URL {queueUrl}.");
                    });
                    return step;
                })
                .SingleInstance()
                .As<IInstallerStep>();

            builder.Register(c => new SqsQueueManager(c.Resolve<IAmazonSQS>()))
                .As<IQueueManager>()
                .InstancePerDependency();
        }
    }
}