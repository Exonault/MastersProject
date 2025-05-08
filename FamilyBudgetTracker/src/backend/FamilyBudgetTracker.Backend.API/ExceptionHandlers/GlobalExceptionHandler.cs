using FamilyBudgetTracker.Backend.API.Messages;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        ProblemDetails problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = ExceptionHandlerMessages.ServerError,
            Detail = exception.Message,
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        // await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
            });


    }
}