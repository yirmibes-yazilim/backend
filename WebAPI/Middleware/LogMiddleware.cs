using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using backend.Loger;

namespace backend.WebAPI.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMyLogger _logger;

        public LogMiddleware(RequestDelegate next, IMyLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip";
            var path = context.Request.Path.Value?.TrimStart('/') ?? "unknown path";
            _logger.LogInformation($"IP: {ip} Request {path}");

            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            if (statusCode >= 200 && statusCode < 300)
            {
                _logger.LogInformation($"Response {path} {statusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            }
            else if (statusCode >= 400 && statusCode < 500)
            {
                _logger.LogWarning($"Response {path} {statusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            }
            else if (statusCode >= 500)
            {
                _logger.LogError($"Response {path} {statusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            }
            else
            {
                _logger.LogInformation($"Response {path} {statusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            }
        }
    }
}
