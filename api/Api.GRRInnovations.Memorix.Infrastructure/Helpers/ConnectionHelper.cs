using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Api.GRRInnovations.Memorix.Infrastructure.Helpers
{
    public class ConnectionHelper
    {
        public static string GetConnectionString(IConfiguration configuration, ILogger? logger = null)
        {
            var connectionString = configuration.GetConnectionString("SqlConnectionString");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            logger?.LogDebug("Checking connection string sources - Config: {HasConfig}, DATABASE_URL: {HasEnv}", 
                !string.IsNullOrEmpty(connectionString), 
                !string.IsNullOrEmpty(databaseUrl));

            return string.IsNullOrEmpty(databaseUrl) 
                ? connectionString 
                : BuildConnectionString(databaseUrl, logger);
        }

        public static string BuildConnectionString(string databaseUrl, ILogger? logger = null)
        {
            try
            {
                var databaseUri = new Uri(databaseUrl);
                var userInfo = databaseUri.UserInfo.Split(':');
                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.LocalPath.TrimStart('/'),
                    SslMode = SslMode.Require,
                    TrustServerCertificate = true
                };

                logger?.LogDebug("Built connection string from DATABASE_URL for host: {Host}, database: {Database}", 
                    builder.Host, 
                    builder.Database);

                return builder.ToString();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to build connection string from DATABASE_URL");
                throw;
            }
        }
    }
}
