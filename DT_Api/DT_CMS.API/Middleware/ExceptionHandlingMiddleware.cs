using System.Net;
using System.Text.Json;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Exceptions;

namespace DT_CMS.API.Middleware;

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
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, response) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, ApiResponseDto.Fail(exception.Message)),
            ValidationException validationEx => (HttpStatusCode.BadRequest, ApiResponseDto.Fail(exception.Message, validationEx.Errors)),
            UnauthorizedException => (HttpStatusCode.Unauthorized, ApiResponseDto.Fail(exception.Message)),
            AppException => (HttpStatusCode.BadRequest, ApiResponseDto.Fail(exception.Message)),
            _ => (HttpStatusCode.InternalServerError, ApiResponseDto.Fail("An unexpected error occurred."))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception occurred");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(payload);
    }
}
