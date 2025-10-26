using Api.GRRInnovations.Memorix.Domain.Exceptions;
using Api.GRRInnovations.Memorix.Domain.Models;
using System.Net;
using System.Text.Json;

namespace Api.GRRInnovations.Memorix.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            Result<object> result;

            switch (ex)
            {
                case DomainException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = Result<object>.Fail(ex.Message, context.TraceIdentifier);

                    _logger.LogWarning(ex, "Domain error: {Message}", ex.Message);
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = Result<object>.Fail("Unexpected erro.", context.TraceIdentifier);

                    _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                    break;
            }

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
