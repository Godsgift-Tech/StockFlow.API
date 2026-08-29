using FluentValidation;
using System.Net;
using System.Text.Json;

namespace StockFlow.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (ValidationException ex)
            {
                _logger.LogWarning(
                    "Validation failed for request {Path}",
                    context.Request.Path);

                context.Response.StatusCode =
                    (int)HttpStatusCode.BadRequest;

                context.Response.ContentType =
                    "application/json";

                var response = new
                {
                    success = false,
                    message = "Validation failed.",
                    errors = ex.Errors
                        .Select(error => new
                        {
                            field = error.PropertyName,
                            message = error.ErrorMessage
                        })
                        .ToList()
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An unexpected error occurred.");

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType =
                    "application/json";

                var response = new
                {
                    success = false,
                    message = "An unexpected error occurred."
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}
