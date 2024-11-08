using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnglishPremierLeague2024.Api
{

    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/problem+json";
            _logger.LogError(exception, "Something went wrong while processing {RequestPath}", httpContext.Request.Path);

            var details = new ValidationProblemDetails
            {
                Detail = exception.Message,
                Instance = httpContext.Request.Path,
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An error occurred while processing request.",
                Type = "Internal Server Error",
                Errors = new Dictionary<string, string[]> { { "Exception", new[] { exception.Message } } }
            };

            switch (exception)
            {
                case NotFoundException _:
                    details.Status = (int)HttpStatusCode.NotFound;
                    details.Type = "Not Found";
                    break;
                case BadRequestException _:
                    details.Status = (int)HttpStatusCode.BadRequest;
                    details.Type = "Bad Request";
                    break;
                case UnauthorizedException _:
                    details.Status = (int)HttpStatusCode.Unauthorized;
                    details.Type = "Unauthorized";
                    break;
                case ForbiddenMethodException _:
                    details.Status = (int)HttpStatusCode.Forbidden;
                    details.Type = "Forbidden";
                    break;
                default:
                    break;
            }

            var response = System.Text.Json.JsonSerializer.Serialize(details, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });

            httpContext.Response.StatusCode = details.Status.Value;

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;

        }
    }

}
