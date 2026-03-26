using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using System.Security.Claims;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        using (LogContext.PushProperty("RequestId", Guid.NewGuid()))
        using (LogContext.PushProperty("UserId", userId ?? "Anonymous"))
        using (LogContext.PushProperty("Path", context.Request.Path))
        using (LogContext.PushProperty("Method", context.Request.Method))
        {
            try
            {
                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation(
                    "HTTP {Method} {Path} responded {StatusCode} in {Elapsed} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds
                );
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex,
                    "HTTP {Method} {Path} failed in {Elapsed} ms",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.ElapsedMilliseconds
                );

                throw;
            }
        }
    }
}