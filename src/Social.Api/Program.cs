using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Autofac;
using Serilog;
using Social.Api.Modules;

namespace Social.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHost(args);
            host.Run();
        }

        public static IHost CreateHost(string[] args)
        {
            var builder = Host
                .CreateDefaultBuilder(args)
                .UseAutofac()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
            var host = builder.Build();

            return host;
        }
    }
}
