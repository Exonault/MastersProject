using FamilyBudgetTracker.Backend.API.Messages;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class ResourceNotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ResourceNotFoundExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    
    public ResourceNotFoundExceptionHandler(ILogger<ResourceNotFoundExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
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
            Title = ExceptionHandlerMessages.NotFound,
            Detail = resourceNotFoundException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
            });
    }
}