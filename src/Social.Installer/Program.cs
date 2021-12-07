using System;
using System.CommandLine;
using System.Threading.Tasks;
using Autofac;
using Library.Installation;
using Microsoft.Extensions.Hosting;
using Social.Infrastructure.Modules;

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
            var builder = Library.Installation.Installer
                .CreateDefaultBuilder()
                //.ConfigureSetupConfiguration(c =>
                //{
                //    c.Properties[HostDefaults.EnvironmentKey] = environment;
                //    c.Properties[HostDefaults.ContentRootKey] = Environment.CurrentDirectory;
                //})
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
