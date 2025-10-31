using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace Api.GRRInnovations.Memorix.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IEndpointRouteBuilder MapApiDocumentation(this IEndpointRouteBuilder endpoints, IHostEnvironment env)
        {
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

            return endpoints;
        }
    }
}

