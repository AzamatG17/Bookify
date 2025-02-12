using Bookify.Domain_.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Middlewares;

public sealed class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger)
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
            await HandleAsync(ex, context);
        }
    }

    private async Task HandleAsync(Exception exception, HttpContext context)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var details = GetErrorDetails(exception);

        context.Response.StatusCode = details.Status!.Value;

        await context.Response
            .WriteAsJsonAsync(details);
    }

    private static ProblemDetails GetErrorDetails(Exception exception)
    => exception switch
    {
        EntityNotFoundException => new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = exception.Message
        },
        UserNameAlreadyTakenException => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "User Name already taken",
            Detail = exception.Message
        },
        ChatIdValidationException => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid Chat ID",
            Detail = exception.Message
        },
        InvalidPasswordException => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid Password",
            Detail = exception.Message
        },
        InvalidUpdateDataException => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid Update Data",
            Detail = exception.Message
        },
        SmsCodeValidationException => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid SMS Code",
            Detail = exception.Message
        },
        UserNameNotExistException => new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "User Name Not Found",
            Detail = exception.Message
        },
        DuplicateBookingException => new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Duplicate Booking",
            Detail = exception.Message
        },
        Domain_.Exceptions.UnauthorizedAccessException => new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Unauthorized Access",
            Detail = exception.Message
        },
        _ => new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error",
            Detail = exception.Message
        }
    };
}
