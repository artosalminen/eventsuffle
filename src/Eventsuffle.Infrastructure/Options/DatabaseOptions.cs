namespace Eventsuffle.Infrastructure.Options
{
    public class DatabaseOptions
    {
        public enum Databases
        {
            None = 0,
            InMemory = 1,
            MicrosoftSql = 2,
        }

        public Databases DatabaseType { get; set; }
        public string DatabaseConnectionString { get; set; }
    }
}
