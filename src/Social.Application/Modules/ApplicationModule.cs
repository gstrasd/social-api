﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Social.Domain;
using Social.Domain.Instagram;

namespace Social.Application.Modules
{
    public class ApplicationModule : Module
    {
        private readonly HostBuilderContext _context;

        public ApplicationModule(HostBuilderContext context)
        {
            _context = context;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new InstagramService(c.Resolve<IOEmbedService>(), c.Resolve<ILogger>()))
                .SingleInstance()
                .As<IInstagramService>();

            builder.Register(c => new CachedInstagramService(c.Resolve<IInstagramService>(), c.Resolve<ILogger>()))
                .SingleInstance()
                .Named<IInstagramService>("cached");
        }
    }
}
