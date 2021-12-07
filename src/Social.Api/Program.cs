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
            var builder = CreateHostBuilder();
            var host = builder.Build();
            host.Run();

            Console.ReadKey();      // TODO: Remove this later once you figure out how to capture exceptions that shut down 
        }

        public static IHostBuilder CreateHostBuilder()
        {
            var hostBuilder = Host
                .CreateDefaultBuilder()
                .UseAutofac()
                .ConfigureContainer((HostBuilderContext context, ContainerBuilder builder) =>
                    {
                        builder.RegisterModule(new ApiModule(context));
                        builder.RegisterModule(new ApplicationModule(context));
                        builder.RegisterModule(new InfrastructureModule(context));
                        builder.RegisterModule(new AwsModule(context));
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return hostBuilder;
        }
    }
}
