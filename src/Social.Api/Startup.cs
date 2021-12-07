using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Library.Configuration;
using Library.Hosting;
using Library.Serilog;
using Social.Api.Modules;
using Social.Application.Modules;
using Social.Infrastructure.Modules;
using Social.Workers.Modules;

namespace Social.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvcCore().AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Social.Api", Version = "v1" });
            });
            services.AddSerilog(options =>
            {
                options.Settings = new ConfigurationLoggerSettings(_configuration);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Social.Api v1"));
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterMessageWorkers(_configuration.Bind<List<MessageWorkerConfiguration>>("MessageWorkers"));
            builder.RegisterModule(new ApiModule(_configuration));
            builder.RegisterModule(new ApplicationModule(_configuration));
            builder.RegisterModule(new InfrastructureModule(_configuration));
            builder.RegisterModule(new WorkersModule(_configuration));
            builder.RegisterModule(new AwsModule(_configuration));
        }
    }
}
