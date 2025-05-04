using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class OperationNotAllowedExceptionHandler : IExceptionHandler
{
    private readonly ILogger<OperationNotAllowedExceptionHandler> _logger;

    public OperationNotAllowedExceptionHandler(ILogger<OperationNotAllowedExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not OperationNotAllowedException invalidOperationException)
        {
            return false;
        }

        _logger.LogError(invalidOperationException, "Exception occurred: {Message}",
            invalidOperationException.Message);

        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad request",
            Detail = invalidOperationException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
