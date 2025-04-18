using FamilyBudgetTracker.Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.ExceptionHandlers;

public class ResourceNotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ResourceNotFoundExceptionHandler> _logger;

    public ResourceNotFoundExceptionHandler(ILogger<ResourceNotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ResourceNotFoundException resourceNotFoundException)
        {
            return false;
        }

        _logger.LogError(resourceNotFoundException, "Exception occurred: {Message}", resourceNotFoundException.Message);

        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not found",
            Detail = resourceNotFoundException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}