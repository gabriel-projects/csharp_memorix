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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Database migrations
            var scope = app.ApplicationServices.CreateScope();
            _ = MigrationHelper.ManageDataAsync(scope.ServiceProvider);

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
