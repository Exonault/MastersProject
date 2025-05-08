using FamilyBudgetTracker.Backend.API.Messages;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;


    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        _logger.LogError(validationException, "Exception occurred: {Message}",
            validationException.Message);

        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = ExceptionHandlerMessages.BadRequest,
            Detail = validationException.Message
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