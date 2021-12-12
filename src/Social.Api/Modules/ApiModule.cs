using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Social.Api.Controllers;
using Social.Domain;
using Social.Domain.Instagram;

namespace Social.Api.Modules
{
    public class ApiModule : Module
    {
        private readonly HostBuilderContext _context;

        public ApiModule(HostBuilderContext context)
        {
            _context = context;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new InstagramController(c.ResolveNamed<IInstagramService>("cached"), c.Resolve<ILogger>()))
                .InstancePerLifetimeScope()
                .AsSelf();

            builder.Register(_ => new JsonSerializerOptions
                {
                    Converters = {new JsonStringEnumConverter()}
                })
                .SingleInstance()
                .AsSelf();
        }
    }
}
