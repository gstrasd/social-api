using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.Extensions.Hosting;
using Library.Autofac;
using Library.Configuration;
using Library.Hosting;
using Library.Serilog;
using Social.Application.Modules;
using Social.Infrastructure.Modules;
using Social.Workers.Modules;

namespace Social.Workers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder();
            var host = builder.Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            var builder = Host
                .CreateDefaultBuilder()
                .UseAutofac()
                .ConfigureServices((context, services) =>
                {
                    services.AddSerilog(options =>
                    {
                        options.Settings = new ConfigurationLoggerSettings(context.Configuration);
                    });
                })
                .ConfigureContainer((HostBuilderContext c, ContainerBuilder cb) =>
                {
                    cb.RegisterMessageWorkers(c.Configuration.Bind<List<MessageWorkerConfiguration>>("MessageWorkers"));
                    cb.RegisterModule(new ApplicationModule(c.Configuration));
                    cb.RegisterModule(new InfrastructureModule(c.Configuration));
                    cb.RegisterModule(new AwsModule(c.Configuration));
                    cb.RegisterModule(new WorkersModule(c.Configuration));
                });

            return builder;
        }
    }
}