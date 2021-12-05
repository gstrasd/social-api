using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Library.Installation;
using Microsoft.Extensions.Configuration;

namespace Social.Infrastructure.Modules
{
    public class AwsSetupModule : Module
    {
        private readonly IConfiguration _configuration;

        public AwsSetupModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var step = new InstallerStep("First step", () => Task.CompletedTask);
                    return step;
                })
                .SingleInstance()
                .As<IInstallerStep>();
        }
    }
}