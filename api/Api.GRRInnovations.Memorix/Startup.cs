using Api.GRRInnovations.Memorix.Infrastructure.Helpers;
using Scalar.AspNetCore;

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
            services.AddControllers();
            services.AddAuthorization();

            ConfigureSwagger(services);
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddOpenApi();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
            }

            //todo: enable migration
            //var scope = app.ApplicationServices.CreateScope();
            //_ = MigrationHelper.ManageDataAsync(scope.ServiceProvider);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Configure the HTTP request pipeline.
                if (env.IsDevelopment())
                {
                    endpoints.MapOpenApi();

                    endpoints.MapScalarApiReference(options =>
                    {
                        options
                            .WithTitle("Memorix API")
                            .WithTheme(ScalarTheme.Mars)
                            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                    });
                }
            });
        }
    }
}
