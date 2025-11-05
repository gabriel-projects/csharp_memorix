using Api.GRRInnovations.Memorix.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

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
                _logger.LogInformation("Applying database migrations...");

                using var scope = _serviceProvider.CreateScope();
                await MigrationHelper.ManageDataAsync(scope.ServiceProvider);

                _logger.LogInformation("Database migrations completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to apply database migrations on startup");
                throw;
            }
            
            return;
        }
    }
}
