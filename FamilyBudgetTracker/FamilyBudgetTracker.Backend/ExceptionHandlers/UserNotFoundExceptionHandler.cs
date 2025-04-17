using FamilyBudgetTracker.Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.ExceptionHandlers;

public class UserNotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UserNotFoundExceptionHandler> _logger;

    public UserNotFoundExceptionHandler(ILogger<UserNotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UserNotFoundException mappingException)
        {
            return false;
        }
        
        _logger.LogError(mappingException, "Exception occurred: {Message}", mappingException.Message);

        ProblemDetails problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not found",
            Detail = mappingException.Message,
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}