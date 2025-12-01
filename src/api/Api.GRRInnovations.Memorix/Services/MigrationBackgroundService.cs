using Api.GRRInnovations.Memorix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.GRRInnovations.Memorix.Services
{
    public class MigrationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<MigrationBackgroundService> _logger;

        public MigrationBackgroundService(
            IServiceProvider serviceProvider,
            IWebHostEnvironment environment,
            ILogger<MigrationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _environment = environment;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Migration check service started.");

                await CheckAndApplyMigrationsAsync();

                _logger.LogInformation("Database migrations completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to apply database migrations on startup");
            }
            
            return;
        }

        private async Task CheckAndApplyMigrationsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pending = await dbContext.Database.GetPendingMigrationsAsync();

            if (pending.Any())
            {
                _logger.LogWarning($"Found {pending.Count()} pending migrations. Applying...");
                await dbContext.Database.MigrateAsync();
                _logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                _logger.LogInformation("No pending migrations found.");
            }
        }
    }
}
