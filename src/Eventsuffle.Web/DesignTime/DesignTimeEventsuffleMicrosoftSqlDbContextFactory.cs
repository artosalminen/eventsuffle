using Eventsuffle.Infrastructure.Data;
using Eventsuffle.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Eventsuffle.Web.DesignTime
{
    public class DesignTimeEventsuffleMicrosoftSqlDbContextFactory : IDesignTimeDbContextFactory<EventSuffleMicrosoftSqlDbContext>
    {
        DatabaseOptions.Databases DbType => DatabaseOptions.Databases.MicrosoftSql;

        class OptionsMock<TOptions> : IOptions<TOptions>
            where TOptions : class, new()
        {
            public OptionsMock(TOptions value)
            {
                Value = value;
            }

            public TOptions Value { get; }
        }

        public EventSuffleMicrosoftSqlDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environment}.json", false, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>()
                .Build();

            var builder = new DbContextOptionsBuilder<EventSuffleDbContext>();

            var dbOptions = new OptionsMock<DatabaseOptions>(new DatabaseOptions()
            {
                DatabaseType = DbType,
                DatabaseConnectionString = configuration[Constants.Keys.ConnectionString(DbType)],
            });

            Console.WriteLine($"Database connectionstring: {dbOptions.Value.DatabaseConnectionString}");

            return (EventSuffleMicrosoftSqlDbContext)Activator.CreateInstance(typeof(EventSuffleMicrosoftSqlDbContext), builder.Options, dbOptions);
        }
    }
}
