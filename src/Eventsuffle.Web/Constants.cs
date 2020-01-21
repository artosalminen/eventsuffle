using Eventsuffle.Infrastructure.Options;

namespace Eventsuffle.Web
{
    /// <summary>
    /// General constants.
    /// </summary>
    public static class Constants
    {
        public static class Formats
        {
            public const string DateFormat = "yyyy-MM-dd";
        }

        public static class Keys
        {
            /// <summary>
            /// Chosen database type. Possible values are represented by enumeration <see cref="DatabaseOptions.Databases"/>
            /// </summary>
            public const string DatabaseType = "DatabaseType";

            /// <summary>
            /// Returns a configuration key for given database type.
            /// </summary>
            /// <param name="dbType"></param>
            /// <returns></returns>
            public static string ConnectionString(DatabaseOptions.Databases dbType) => $"ConnectionStrings:{dbType}";
        }
    }
}
