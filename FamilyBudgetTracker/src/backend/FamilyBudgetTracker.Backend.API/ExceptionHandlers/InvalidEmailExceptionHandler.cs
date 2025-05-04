using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class InvalidEmailExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InvalidEmailExceptionHandler> _logger;

    public InvalidEmailExceptionHandler(ILogger<InvalidEmailExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InvalidEmailPasswordException invalidEmailPasswordException)
        {
            return false;
        }

        _logger.LogError(invalidEmailPasswordException, "Exception occurred: {Message}",
            invalidEmailPasswordException.Message);

        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = invalidEmailPasswordException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}