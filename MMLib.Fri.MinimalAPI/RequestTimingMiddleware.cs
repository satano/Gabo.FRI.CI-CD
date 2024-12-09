namespace MMLib.Fri.MinimalAPI;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        await _next(context);

        watch.Stop();
        _logger.LogInformation("Request {Method} {Path} took {Duration}ms",
            context.Request.Method,
            context.Request.Path,
            watch.ElapsedMilliseconds);
    }
}

public static class RequestTimingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestTimingMiddleware>();
}