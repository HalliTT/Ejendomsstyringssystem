using AuthService.Contracts;
using System.Diagnostics;

namespace AuthService.Api.Middleware
{
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception. TraceId: {TraceId}, Path: {Path}, Method: {Method}",
                    traceId,
                    context.Request.Path,
                    context.Request.Method);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var payload = ApiResponse.ErrorResponse(
                    "server_error",
                    $"Request failed. Reference: {traceId}");

                await context.Response.WriteAsJsonAsync(payload);
            }
        }
    }
}