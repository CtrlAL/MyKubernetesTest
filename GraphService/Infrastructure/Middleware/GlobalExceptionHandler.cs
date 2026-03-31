using System.Net;
using GraphService.Domain.Exceptions;
using GraphService.Domain.Shared;
using Microsoft.AspNetCore.Diagnostics;

namespace GraphService.Infrastructure.Middleware
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

            var (statusCode, failure) = exception switch
            {
                AppException appEx => (
                    appEx.StatusCode,
                    Failure.Create(appEx.Message)),

                _ => (
                    HttpStatusCode.InternalServerError,
                    Failure.Create("An unexpected error occurred."))
            };

            httpContext.Response.StatusCode = (int)statusCode;

            var result = Result.Fail(failure);
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

            return true;
        }
    }
}
