using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Offices.Models.HttpExceptions;
using Offices.Models.Settings;

namespace Offices.Services.Middleware;

/// <summary>
/// Глобальный перехватчик исключений. 
/// Обеспечивает единый формат ответа API при возникновении ошибок.
/// </summary>
internal class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RuntimeSettings _settings;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="next">Следующий в конвейере запросов.</param>
    /// <param name="logger">Логгер для записи необработанных ошибок.</param>
    /// <param name="options">Настройки приложения (IOptions для DI).</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger,
        IOptions<RuntimeSettings> options)
    {
        _next = next;
        _logger = logger;_settings = options.Value;
    }

    /// <summary>
    /// Вызов следующего middleware и перехватывает возможные исключения.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Формирует JSON-ответ в зависимости от типа исключения.
    /// </summary>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError; // 500 по умолчанию
        object resultPayload;
        
        if (exception is HttpException httpEx)
        {
            // если это кастомная ошибка (например, 401 или 403)
            statusCode = (HttpStatusCode) httpEx.StatusCode;

            resultPayload = new
            {
                StatusCode = statusCode,
                Message = httpEx.Message,
                Code = httpEx.Code,
                Data = httpEx.PayloadData
            };
        }
        else
        {
            // какая-то ошибка (баг, БД упала и т.д.)
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            
            resultPayload = new 
            {
                StatusCode = 500,
                Message = _settings.DevMode ? exception.Message : "Internal Server Error",
                
                // Details (StackTrace) пробрасываем ТОЛЬКО в режиме разработки, ибо светить
                // структуру папок на беке в проде - не оч
                Details = _settings.DevMode ? exception.StackTrace : null 
            };
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(resultPayload, jsonOptions));
    }
}