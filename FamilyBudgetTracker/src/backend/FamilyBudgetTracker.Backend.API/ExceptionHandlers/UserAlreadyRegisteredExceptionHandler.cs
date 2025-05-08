using FamilyBudgetTracker.Backend.API.Messages;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class UserAlreadyRegisteredExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UserAlreadyRegisteredExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    
    public UserAlreadyRegisteredExceptionHandler(ILogger<UserAlreadyRegisteredExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
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
            Title = ExceptionHandlerMessages.Conflict,
            Detail = userAlreadyRegisteredException.Message,
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