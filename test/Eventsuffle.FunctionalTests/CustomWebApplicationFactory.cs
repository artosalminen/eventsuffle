using Eventsuffle.Infrastructure.Options;
using Eventsuffle.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Eventsuffle.FunctionalTests
{
    /// <summary>
    /// Use Custom Web Application Factory to initialize web app configurations and the DB.
    /// </summary>
    /// <remarks>See: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#customize-webapplicationfactory for details.</remarks>
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
       where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = base.CreateHostBuilder();

            builder.ConfigureAppConfiguration(configBuilder =>
            {
                var config = configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Testing.json", optional: false)
                    .Build();

                Enum.TryParse(config.GetValue<string>(Constants.Keys.DatabaseType), true,
                            out DatabaseOptions.Databases database);

                if (database == DatabaseOptions.Databases.InMemory)
                {
                    // Randomize inmemory database to be different for each test run.
                    config[Constants.Keys.ConnectionString(DatabaseOptions.Databases.InMemory)] = Guid.NewGuid().ToString();
                }

            });

            return builder;
        }
    }
}
