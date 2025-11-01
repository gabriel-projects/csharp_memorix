using Api.GRRInnovations.Memorix.Application.Interfaces;
using Api.GRRInnovations.Memorix.Application.Interfaces.Persistence;
using Api.GRRInnovations.Memorix.Application.Interfaces.Services;
using Api.GRRInnovations.Memorix.Domain.Models;
using Api.GRRInnovations.Memorix.Infrastructure.Helpers;
using Api.GRRInnovations.Memorix.Infrastructure.Persistence;
using Api.GRRInnovations.Memorix.Infrastructure.Persistence.Repositories;
using Api.GRRInnovations.Memorix.Infrastructure.Security;
using Api.GRRInnovations.Memorix.Infrastructure.Security.Crypto;
using Api.GRRInnovations.Memorix.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.GRRInnovations.Memorix.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IDeckRepository, DeckRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IJwtService, JwtService>();

            
            services.AddScoped<IUserContext, UserContext>();
            services.AddHttpContextAccessor();

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>((serviceProvider, opt) =>
                {
                    opt.UseInMemoryDatabase("MemorixDb");
                });
            }
            else
            {
                AddDbContext(services, configuration);
            }
        }

        public static IServiceCollection AddInfrastructureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);

            var jwtSettings = new JwtSettings();
            jwtSection.Bind(jwtSettings);

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger(typeof(DependencyInjection));

            string connection = GetConnectionStringSQL(configuration, logger);

            logger.LogDebug("Database connection configured successfully");
            logger.LogDebug("Using {ConnectionSource}", 
                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL")) 
                    ? "appsettings connection string" 
                    : "DATABASE_URL environment variable");

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(connection);
            });
        }

        private static string GetConnectionStringSQL(IConfiguration configuration, ILogger logger)
        {
            var connectionString = configuration.GetConnectionString("SqlConnectionString");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            logger.LogDebug("SQL Connection String configured: {HasConnectionString}", !string.IsNullOrEmpty(connectionString));
            logger.LogDebug("DATABASE_URL from environment: {HasDatabaseUrl}", !string.IsNullOrEmpty(databaseUrl));

            var connection = string.IsNullOrEmpty(databaseUrl) 
                ? connectionString 
                : ConnectionHelper.BuildConnectionString(databaseUrl, logger);
            
            return connection;
        }
    }
}
