using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class MappingExceptionHandler : IExceptionHandler
{
    private readonly ILogger<MappingExceptionHandler> _logger;

    public MappingExceptionHandler(ILogger<MappingExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not MappingException mappingException)
        {
            return false;
        }

        _logger.LogError(mappingException, "Exception occurred: {Message}", mappingException.Message);

        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = mappingException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}