using FamilyBudgetTracker.BE.Commons.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.ExceptionHandlers;

public class UserAlreadyRegisteredExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UserAlreadyRegisteredExceptionHandler> _logger;

    public UserAlreadyRegisteredExceptionHandler(ILogger<UserAlreadyRegisteredExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UserAlreadyRegisteredException userAlreadyRegisteredException)
        {
            return false;
        }


        _logger.LogError(userAlreadyRegisteredException, "Exception occurred: {Message}", userAlreadyRegisteredException.Message);

        ProblemDetails problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflict",
            Detail = userAlreadyRegisteredException.Message,
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}