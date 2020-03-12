using System;
using Eventsuffle.Core.Entities;
using Eventsuffle.Core.Interfaces;
using Eventsuffle.Core.Services;
using Eventsuffle.Infrastructure.Data;
using Eventsuffle.Web.Converters;
using Eventsuffle.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Eventsuffle
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning();

            // The specified format code will format the version as "'v'major[.minor][-status]".
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.ConfigureOptions(Configuration);
            services.ConfigureSwagger();
            services.ConfigureApiVersioning();
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new IsoDateConverter());
                });

            services.ConfigureDatabaseContexts();
            services.AddScoped<IAsyncRepository<Event, Guid>, EfRepository<Event, Guid>>();
            services.AddScoped<IAsyncRepository<Vote, Guid>, EfRepository<Vote, Guid>>();
            services.AddScoped<IAsyncRepository<SuggestedDate, Guid>, EfRepository<SuggestedDate, Guid>>();
            services.AddScoped<IAsyncRepository<VoteSuggestedDate, Guid>, EfRepository<VoteSuggestedDate, Guid>>();

            services.AddScoped<IEventService, EventService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider versionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.ConfigureSwagger(versionDescriptionProvider);
            }
        }
    }
}
