using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetTracker.Backend.API.ExceptionHandlers;

public class BadHttpRequestExceptionHandler : IExceptionHandler
{
    private readonly ILogger<BadHttpRequestExceptionHandler> _logger;

    public BadHttpRequestExceptionHandler(ILogger<BadHttpRequestExceptionHandler> logger)
    {
        _logger = logger;
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
            Title = "Bad Request",
            Detail = badHttpRequest.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}