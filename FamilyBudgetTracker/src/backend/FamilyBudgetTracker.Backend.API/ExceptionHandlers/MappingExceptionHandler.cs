using FamilyBudgetTracker.Backend.API.Messages;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class MappingExceptionHandler : IExceptionHandler
{
    private readonly ILogger<MappingExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    
    public MappingExceptionHandler(ILogger<MappingExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
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
            Title = ExceptionHandlerMessages.BadRequest,
            Detail = mappingException.Message
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