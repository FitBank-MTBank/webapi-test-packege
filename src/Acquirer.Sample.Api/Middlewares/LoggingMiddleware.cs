using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Acquirer.Sample.Api.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        _logger.LogInformation($"{request.Method} {request.Path}");

        if (request.Body.CanRead)
        {
            request.EnableBuffering();
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            requestBody = requestBody.Replace("\n", "").Replace("\r", "");
            if (!string.IsNullOrWhiteSpace(requestBody)) _logger.LogInformation($"{requestBody}");
            request.Body.Position = 0;
        }

        var originalStream = context.Response.Body;
        using var stream = new MemoryStream();
        context.Response.Body = stream;

        await _next(context);

        var response = context.Response;

        stream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(stream).ReadToEndAsync();
        if (!request.Path.ToString().Contains("swagger")) _logger.LogInformation($"{response.StatusCode} - {responseBody}");

        stream.Seek(0, SeekOrigin.Begin);
        await stream.CopyToAsync(originalStream);
        context.Response.Body = originalStream;
    }
}