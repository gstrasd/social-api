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
using Social.Application.Modules;

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
                .ConfigureContainer((HostBuilderContext c, ContainerBuilder cb) =>
                    {
                        cb.RegisterModule(new ApiModule(c.Configuration));
                        cb.RegisterModule(new ApplicationModule(c.Configuration));
                        cb.RegisterModule(new InfrastructureModule(c.Configuration));
                        cb.RegisterModule(new AwsModule(c.Configuration));
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return builder;
        }

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
    }
}
