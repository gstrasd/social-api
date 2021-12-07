using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Library.Autofac;
using Library.Hosting;
using Serilog;
using Social.Api.Modules;
using Library.Installation;
using Social.Infrastructure.Modules;

namespace Social.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var command = new HostCommand(CreateHostBuilder(args));
            command.AddCommand(new InstallerCommand(CreateInstallerBuilder()));
            command.Invoke(args);

            Console.ReadKey();      // TODO: Remove this later once you figure out how to capture exceptions that shut down 
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host
                .CreateDefaultBuilder(args)
                .UseAutofac()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return builder;
        }

        // TODO: Decide if this should be a separate executable
        public static IInstallerBuilder CreateInstallerBuilder()
        {
            var builder = Installer
                .CreateDefaultBuilder()
                .UseAppSettings()
                .ConfigureContainer((context, container) =>
                {
                    container.RegisterModule(new AwsSetupModule(context.Configuration));
                    container.RegisterModule(new AwsModule(context.Configuration));
                });

            return builder;
        }

        // TODO: Decide how to create just a worker executable. Either by using a WorkerCommand, or by making the Social.Workers project an executable
        public static IHostBuilder CreateWorkerBuilder(string[] args)
        {
            var builder = Host
                .CreateDefaultBuilder(args)
                .UseAutofac()
                .ConfigureContainer((HostBuilderContext c, ContainerBuilder cb) =>
                {
                    cb.RegisterModule(new AwsSetupModule(c.Configuration));
                    cb.RegisterModule(new AwsModule(c.Configuration));
                });

            return builder;
        }
    }
}
