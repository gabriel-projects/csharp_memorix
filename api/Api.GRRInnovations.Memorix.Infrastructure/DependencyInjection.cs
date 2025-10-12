using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Infrastructure.Helpers;
using Api.GRRInnovations.Memorix.Infrastructure.Persistence;
using Api.GRRInnovations.Memorix.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDeckRepository, DeckRepository>();

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase("MemorixDb"));

            //todo: it is not yet possible to test this because the database is not yet created
            //AddDbContext(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            string connection = GetConnectionStringSQL(configuration);

            Console.WriteLine($"connection Startup: {connection}");

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));
        }

        private static string GetConnectionStringSQL(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnectionString");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            Console.WriteLine($"sqlConnection Startup: {connectionString}");
            Console.WriteLine($"databaseUrl Startup: {databaseUrl}");

            var connection = string.IsNullOrEmpty(databaseUrl) ? connectionString : ConnectionHelper.BuildConnectionString(databaseUrl);
            return connection;
        }
    }
}
