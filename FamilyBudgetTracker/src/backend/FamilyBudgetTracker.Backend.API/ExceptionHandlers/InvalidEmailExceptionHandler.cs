using FamilyBudgetTracker.Backend.API.Messages;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class InvalidEmailExceptionHandler : IExceptionHandler
{
    private readonly ILogger<InvalidEmailExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;


    public InvalidEmailExceptionHandler(ILogger<InvalidEmailExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
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
            Title = ExceptionHandlerMessages.BadRequest,
            Detail = invalidEmailPasswordException.Message
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