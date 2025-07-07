using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using chatbot.Domain.Exceptions;

namespace chatbot.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Validation failed";
                errorResponse.Details = validationEx.Message;
                errorResponse.Errors = validationEx.Errors;
                _logger.LogWarning(exception, "ValidationException occurred: {Message}", validationEx.Message);
                break;

            case ChatbotException chatbotEx:
                response.StatusCode = chatbotEx.StatusCode;
                errorResponse.Message = chatbotEx.Message;
                _logger.LogWarning(exception, "ChatbotException occurred: {Message}", chatbotEx.Message);
                break;

            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Invalid argument provided";
                errorResponse.Details = argEx.Message;
                _logger.LogWarning(exception, "ArgumentException occurred: {Message}", argEx.Message);
                break;

            case InvalidOperationException invalidOpEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Invalid operation";
                errorResponse.Details = invalidOpEx.Message;
                _logger.LogWarning(exception, "InvalidOperationException occurred: {Message}", invalidOpEx.Message);
                break;

            case UnauthorizedAccessException unauthorizedEx:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = "Unauthorized access";
                errorResponse.Details = unauthorizedEx.Message;
                _logger.LogWarning(exception, "UnauthorizedAccessException occurred: {Message}", unauthorizedEx.Message);
                break;

            case KeyNotFoundException keyNotFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = "Resource not found";
                errorResponse.Details = keyNotFoundEx.Message;
                _logger.LogWarning(exception, "KeyNotFoundException occurred: {Message}", keyNotFoundEx.Message);
                break;

            case TimeoutException timeoutEx:
                response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                errorResponse.Message = "Request timeout";
                errorResponse.Details = timeoutEx.Message;
                _logger.LogWarning(exception, "TimeoutException occurred: {Message}", timeoutEx.Message);
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An unexpected error occurred";
                
                // Apenas inclui detalhes da exceção no ambiente de desenvolvimento
                if (_environment.IsDevelopment())
                {
                    errorResponse.Details = exception.Message;
                    errorResponse.StackTrace = exception.StackTrace;
                }
                
                _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
                break;
        }

        errorResponse.StatusCode = response.StatusCode;
        errorResponse.Timestamp = DateTime.UtcNow;
        errorResponse.Path = context.Request.Path;
        errorResponse.Method = context.Request.Method;

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string? StackTrace { get; set; }
    public DateTime Timestamp { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public IReadOnlyDictionary<string, string[]>? Errors { get; set; }
}
