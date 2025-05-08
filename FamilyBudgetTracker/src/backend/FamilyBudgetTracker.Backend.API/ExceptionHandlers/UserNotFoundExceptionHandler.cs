using FamilyBudgetTracker.Backend.API.Messages;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class UserNotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UserNotFoundExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    
    public UserNotFoundExceptionHandler(ILogger<UserNotFoundExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
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
            Title = ExceptionHandlerMessages.NotFound,
            Detail = mappingException.Message,
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