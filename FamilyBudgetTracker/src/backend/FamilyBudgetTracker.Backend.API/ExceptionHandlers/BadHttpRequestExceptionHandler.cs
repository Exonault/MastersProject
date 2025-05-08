using FamilyBudgetTracker.Backend.API.Messages;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class BadHttpRequestExceptionHandler : IExceptionHandler
{
    private readonly ILogger<BadHttpRequestExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;


    public BadHttpRequestExceptionHandler(ILogger<BadHttpRequestExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BadHttpRequestException badHttpRequest)
        {
            return false;
        }

        // _logger.LogError(badHttpRequest, "Exception occurred: {Message}",
        //     badHttpRequest.Message);

        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status402PaymentRequired,
            Title =  ExceptionHandlerMessages.BadRequest,
            Detail = badHttpRequest.Message
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