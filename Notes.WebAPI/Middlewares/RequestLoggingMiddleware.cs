using System.Text;

namespace Notes.WebAPI.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log request information
        _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

        // Log request body (if any)
        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                var requestBody = await reader.ReadToEndAsync();
                _logger.LogInformation($"Request body: {requestBody}");
                context.Request.Body.Position = 0; // Reset the request body stream position
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
