using Api.GRRInnovations.Memorix.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("SqlConnectionString");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // Criar logger para design-time (migrations)
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
            var logger = loggerFactory.CreateLogger<ApplicationDbContextFactory>();

            logger.LogDebug("Design-time DbContext factory - Config connection: {HasConfig}, DATABASE_URL: {HasEnv}", 
                !string.IsNullOrEmpty(connectionString), 
                !string.IsNullOrEmpty(databaseUrl));

            var connection = string.IsNullOrEmpty(databaseUrl) 
                ? connectionString 
                : ConnectionHelper.BuildConnectionString(databaseUrl, logger);

            logger.LogDebug("Database connection configured for design-time operations");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connection);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
