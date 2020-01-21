using Eventsuffle.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Eventsuffle.Infrastructure.Data
{
    public class EventSuffleMicrosoftSqlDbContext : EventSuffleDbContext
    {
        /// <summary>
        /// Represents a Microsoft SQL Database context.
        /// </summary>
        public EventSuffleMicrosoftSqlDbContext(DbContextOptions<EventSuffleDbContext> options,
            IOptions<DatabaseOptions> dbOptions) : base(options, dbOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(DbOptions.DatabaseConnectionString);
    }
}
