using System;
using System.CommandLine;
using System.Threading.Tasks;
using Autofac;
using Library.Installation;
using Microsoft.Extensions.Hosting;
using Social.Infrastructure.Modules;
using Social.Installer.Modules;

namespace Social.Installer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var installer = new InstallerCommand(CreateInstallerBuilder());
            await installer.InvokeAsync(args);
        }

        public static IInstallerBuilder CreateInstallerBuilder()
        {
            var installerBuilder = Library.Installation.Installer
                .CreateDefaultBuilder()
                .UseAppSettings()
                .ConfigureContainer((context, builder) =>
                {
                    builder.RegisterModule(new AwsSetupModule(context));
                    builder.RegisterModule(new AwsModule(context));
                });

            return installerBuilder;
        }
    }
}
