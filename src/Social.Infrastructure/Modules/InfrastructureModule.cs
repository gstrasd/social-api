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
using Social.Domain.Twitter;
using Social.Infrastructure.Iframely;
using Social.Infrastructure.Twitter;

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
            /* IFramely */
            builder.Register(_ => new HttpClient())
                .SingleInstance()
                .Named<HttpClient>("iframely");

            builder.Register(_ => _configuration.Bind<IframelyConfiguration>("Iframely"))
                .SingleInstance()
                .AsSelf();

            builder.Register(c => new IframelyService(c.ResolveNamed<HttpClient>("iframely"), c.Resolve<IframelyConfiguration>(), c.Resolve<ILogger>()))
                .SingleInstance()
                .As<IOEmbedService>();

            /* Instagram */
            builder.Register(_ => new HttpClient())
                .SingleInstance()
                .Named<HttpClient>("instagram");

            /* Twitter */
            builder.Register(_ => new HttpClient())
                .SingleInstance()
                .Named<HttpClient>("twitter");

            builder.Register(c =>
                {
                    var configuration = _configuration.Bind<TwitterConfiguration>("Twitter");
                    return new TwitterService(c.ResolveNamed<HttpClient>("twitter"), configuration, c.Resolve<ILogger>());
                })
                .SingleInstance()
                .As<ITwitterService>();
        }
    }
}
