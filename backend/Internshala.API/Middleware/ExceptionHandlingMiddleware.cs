using System.Text.Json;
using Internshala.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Internshala.API.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.TraceIdentifier;

        var (statusCode, title, errors) = exception switch
        {
            ValidationException ve => (422, "Validation Error",
                ve.Errors.SelectMany(e => e.Value.Select(m => $"{e.Key}: {m}")).ToArray()),
            NotFoundException nfe => (404, "Not Found", new[] { nfe.Message }),
            ConflictException ce => (409, "Conflict", new[] { ce.Message }),
            ForbiddenException fe => (403, "Forbidden", new[] { fe.Message }),
            UnauthorizedException ue => (401, "Unauthorized", new[] { ue.Message }),
            _ => (500, "Internal Server Error", new[] { "An unexpected error occurred." })
        };

        if (statusCode == 500)
        {
            logger.LogError(exception, "Unhandled exception. CorrelationId: {CorrelationId}", correlationId);
        }

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = $"https://httpstatuses.com/{statusCode}",
            Extensions = { ["correlationId"] = correlationId, ["errors"] = errors }
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
