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
using Social.Workers.Modules;

namespace Social.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var command = new HostCommand(CreateHostBuilder(args));
            command.AddCommand(new InstallerCommand(CreateInstallerBuilder()));
            command.Invoke(args);
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

        public static IInstallerBuilder CreateInstallerBuilder()
        {
            var builder = Installer
                .CreateDefaultBuilder()
                .ConfigureContainer((context, container) =>
                {
                    container.RegisterModule(new AwsSetupModule(context.Configuration));
                    container.RegisterModule(new AwsModule(context.Configuration));
                });

            return builder;
        }
    }
}
