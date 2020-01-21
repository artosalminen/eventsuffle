using Eventsuffle.Infrastructure.Data;
using Eventsuffle.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;

namespace Eventsuffle.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureDatabaseContexts(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            DatabaseOptions dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            void AddContext<TDbImplementation>() where TDbImplementation : EventSuffleDbContext
            {
                // Add migration db context.
                if (dbOptions.DatabaseType != DatabaseOptions.Databases.InMemory)
                {
                    services.AddDbContext<TDbImplementation>();
                }

                // Add Database specific connection configuration for base db context, which is used by application logic.
                services.AddScoped(c =>
                {
                    var options = new DbContextOptionsBuilder<EventSuffleDbContext>();
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    switch (dbOptions.DatabaseType)
                    {
                        case DatabaseOptions.Databases.InMemory:
                            options.UseInMemoryDatabase(dbOptions.DatabaseConnectionString);
                            break;
                        case DatabaseOptions.Databases.MicrosoftSql:
                            options.UseSqlServer(dbOptions.DatabaseConnectionString);
                            break;
                    }
                    return options.Options;
                });

                // Add base identity context as it will be the one used by application logic.
                // Database implementation specific context types are only needed for migrations.
                services.AddDbContext<EventSuffleDbContext, TDbImplementation>();
            }

            // Depending on database type, Add correct type of migration database context.
            // Migrations will be run using database specific database context.
            switch (dbOptions.DatabaseType)
            {
                case DatabaseOptions.Databases.InMemory:
                    AddContext<EventSuffleDbContext>();
                    break;
                case DatabaseOptions.Databases.MicrosoftSql:
                    AddContext<EventSuffleMicrosoftSqlDbContext>();
                    break;
            }

            return services;
        }

        internal static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions()
                .Configure<DatabaseOptions>(options =>
                {
                    Enum.TryParse(configuration.GetValue<string>(Constants.Keys.DatabaseType), true,
                        out DatabaseOptions.Databases database);

                    var connectionString = database != DatabaseOptions.Databases.None
                        ? configuration.GetValue(Constants.Keys.ConnectionString(database), string.Empty)
                        : string.Empty;

                    options.DatabaseType = database;
                    options.DatabaseConnectionString = connectionString;
                });
            return services;
        }

        internal static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opts =>
            {
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
            });
            return services;
        }

        internal static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Resolve the IApiVersionDescriptionProvider service with a temporary service.
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                // Add a swagger document for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateSwaggerInfoForApiVersion(description));
                }
            });
            return services;
        }

        private static OpenApiInfo CreateSwaggerInfoForApiVersion(ApiVersionDescription description)
        {

            var info = new OpenApiInfo()
            {
                Title = $"Event Suffle API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
            };

            if (description.IsDeprecated)
            {
                info.Description += " (deprecated).";
            }

            return info;
        }
    }
}
