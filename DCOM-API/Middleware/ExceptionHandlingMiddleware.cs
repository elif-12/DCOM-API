using System.Net;
using System.Text.Json;

namespace DCOM_API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Sıradaki adıma devam et (controller vs.)
            await _next(context);
        }
        catch (Exception ex)
        {
            // Hatayı console'a logla
            _logger.LogError(ex, "Beklenmeyen bir hata olustu: {Message}", ex.Message);

            // Uygun response'u üret
            await WriteErrorResponseAsync(context, ex);
        }
    }

    private static Task WriteErrorResponseAsync(HttpContext context, Exception ex)
    {
        // Hata türüne göre HTTP kodu seç
        var statusCode = ex switch
        {
            InvalidOperationException => (int)HttpStatusCode.BadRequest,      // 400
            KeyNotFoundException => (int)HttpStatusCode.NotFound,         // 404
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,  // 401
            _ => (int)HttpStatusCode.InternalServerError                      // 500
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var payload = JsonSerializer.Serialize(new
        {
            statusCode,
            message = ex.Message
        });

        return context.Response.WriteAsync(payload);
    }
}