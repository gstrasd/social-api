using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Library.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;
using Social.Domain;
using Social.Infrastructure.Iframely;

namespace Social.Infrastructure.Modules
{
    public class InfrastructureModule : Module
    {
        private readonly IConfiguration _configuration;

        public InfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new HttpClient())
                .SingleInstance()
                .Named<HttpClient>("iframely");

            builder.Register(_ => _configuration.Bind<IframelyConfiguration>("Iframely"))
                .SingleInstance()
                .AsSelf();

            builder.Register(c => new IframelyService(c.ResolveNamed<HttpClient>("iframely"), c.Resolve<IframelyConfiguration>(), c.Resolve<ILogger>()))
                .SingleInstance()
                .As<IOEmbedService>();
        }
    }
}
