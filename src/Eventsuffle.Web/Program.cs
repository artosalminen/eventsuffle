using System;
using System.Linq;
using Eventsuffle.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Eventsuffle
{
    public class Program
    {
        public static IConfigurationBuilder ConfigurationBuilder { get; private set; }

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IWebHostEnvironment>();
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var dbContext = services.GetRequiredService<EventSuffleDbContext>();
                    if (env.IsDevelopment() && !dbContext.Events.Any())
                    {
                        EventSuffleDbContextSeed.SeedDevelopmentAsync(dbContext).Wait();
                    }
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, configurationBuilder) =>
                {
                    var env = builderContext.HostingEnvironment;

                    ConfigurationBuilder = configurationBuilder
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddJsonFile("appsettings.secret.json", true, true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<Startup>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog(
                    (context, configuration) =>
                    {
                        var appConfig = ConfigurationBuilder.Build();
                        configuration.ReadFrom.Configuration(appConfig);
                        var env = context.HostingEnvironment;
                        if (env.IsDevelopment() || env.IsStaging())
                        {
                            configuration.MinimumLevel.Debug();
                        }
                        else
                        {
                            configuration.MinimumLevel.Information();
                        }
                    });
    }
}
