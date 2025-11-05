using Api.GRRInnovations.Memorix.Application;
using Api.GRRInnovations.Memorix.Extensions;
using Api.GRRInnovations.Memorix.Infrastructure;
using Api.GRRInnovations.Memorix.Infrastructure.Helpers;
using Api.GRRInnovations.Memorix.Middlewares;

namespace Api.GRRInnovations.Memorix
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Application Layer
            services.AddApplicationServices();

            // Infrastructure Layer
            services.AddInfrastructureServices(Configuration);
            services.AddInfrastructureAuthentication(Configuration);

            // API Layer
            services.AddApiServices();
            services.AddApiDocumentation();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            try
            {
                using var scope = app.ApplicationServices.CreateScope();

                if (env.IsDevelopment() || env.IsEnvironment("Migration"))
                {
                    logger.LogInformation("Applying database migrations...");
                    await MigrationHelper.ManageDataAsync(scope.ServiceProvider);
                    logger.LogInformation("Database migrations completed successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to apply database migrations on startup");
                throw;
            }

            // Middleware pipeline
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapApiDocumentation(env);
            });
        }
    }
}
