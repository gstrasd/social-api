﻿using System;
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
            var hostBuilder = Host
                .CreateDefaultBuilder()
                .UseAutofac()
                .ConfigureServices((context, services) =>
                {
                    services.AddSerilog(options =>
                    {
                        options.Settings = new ConfigurationLoggerSettings(context.Configuration);
                    });
                })
                .ConfigureContainer((HostBuilderContext context, ContainerBuilder builder) =>
                {
                    builder.RegisterMessageWorkers(context.Configuration.Bind<List<MessageWorkerConfiguration>>("MessageWorkers"));
                    builder.RegisterModule(new ApplicationModule(context));
                    builder.RegisterModule(new InfrastructureModule(context));
                    builder.RegisterModule(new AwsModule(context));
                    builder.RegisterModule(new WorkersModule(context));
                });

            return hostBuilder;
        }
    }
}