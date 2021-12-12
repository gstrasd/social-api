using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.SQS;
using Autofac;
using Library.Amazon;
using Library.Installation;
using Library.Platform.Queuing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Social.Installer.Modules
{
    public class AwsSetupModule : Module
    {
        private readonly HostBuilderContext _context;

        public AwsSetupModule(HostBuilderContext context)
        {
            _context = context;
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

            // Create SearchHistory table
            builder.Register(c =>
                {
                    var client = c.Resolve<IAmazonDynamoDB>();
                    var step = new InstallerStep("Create SearchHistory table", async () =>
                    {
                        var listTablesResponse = await client.ListTablesAsync();
                        if (listTablesResponse.TableNames.Contains("SearchHistory"))
                        {
                            Console.WriteLine("SearchHistory table already exists.");
                        }
                        else
                        {
                            var request = new CreateTableRequest
                            {
                                TableName = "SearchHistory",
                                KeySchema = new List<KeySchemaElement>
                                {
                                    new() { AttributeName = "Value", KeyType = "HASH" },
                                    new() { AttributeName = "Type", KeyType = KeyType.RANGE }
                                },
                                AttributeDefinitions = new List<AttributeDefinition>
                                {
                                    new() { AttributeName = "Value", AttributeType = ScalarAttributeType.S },
                                    new() { AttributeName = "Type", AttributeType = ScalarAttributeType.N },
                                    new() { AttributeName = "Success", AttributeType = ScalarAttributeType.N }
                                },
                                ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 1, WriteCapacityUnits = 1 }
                            };

                            await client.CreateTableAsync(request);
                            Console.WriteLine("SearchHistory table created.");
                        }
                    });

                    return step;
                })
                .SingleInstance()
                .As<InstallerStep>();

            builder.Register(c => new SqsQueueManager(c.Resolve<IAmazonSQS>()))
                .As<IQueueManager>()
                .InstancePerDependency();
        }
    }
}