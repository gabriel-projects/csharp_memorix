using Api.GRRInnovations.Memorix.Application;
using Api.GRRInnovations.Memorix.Extensions;
using Api.GRRInnovations.Memorix.Infrastructure;
using Api.GRRInnovations.Memorix.Middlewares;
using Api.GRRInnovations.Memorix.Services;

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

            // Background Services
            services.AddHostedService<MigrationBackgroundService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Middleware pipeline
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
