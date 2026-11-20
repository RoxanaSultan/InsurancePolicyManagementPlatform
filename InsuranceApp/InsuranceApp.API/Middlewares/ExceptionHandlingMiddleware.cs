using InsuranceApp.Application.Exceptions;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace InsuranceApp.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong in {Path}", httpContext.Request.Path);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();
        var statusCode = HttpStatusCode.InternalServerError;

        switch (ex)
        {
            case ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                response.ErrorType = "ValidationError";
                response.ErrorMessage = "One or more validation errors occurred.";
                response.Errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());
                break;

            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                response.ErrorType = "NotFound";
                response.ErrorMessage = ex.Message;
                break;

            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                response.ErrorType = "BadRequest";
                response.ErrorMessage = ex.Message;
                break;

            case ConflictException:
                statusCode = HttpStatusCode.Conflict;
                response.ErrorType = "Conflict";
                response.ErrorMessage = ex.Message;
                break;

            case UnauthorizedException:
                statusCode = HttpStatusCode.Unauthorized;
                response.ErrorType = "Unauthorized";
                response.ErrorMessage = ex.Message;
                break;

            default:
                response.ErrorType = "ServerError";
                response.ErrorMessage = "An unexpected error occurred.";
                break;
        }

        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}