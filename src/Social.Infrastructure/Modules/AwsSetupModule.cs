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

        protected override void Load(ContainerBuilder builder)
        {
            // Create process-instagram-account queue
            builder.Register(c =>
                {
                    var manager = c.Resolve<IQueueManager>();
                    var step = new InstallerStep("Create process-instagram-account queue", async () =>
                    {
                        var exists = await manager.QueueExistsAsync("process-instagram-account");
                        if (exists)
                        {
                            Console.WriteLine("Queue already exists.");
                            return;
                        }
                        var queueUrl = await manager.CreateQueueAsync("process-instagram-account");
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