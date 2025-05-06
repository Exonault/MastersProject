using FamilyBudgetTracker.Backend.Authentication.Constants;
using Microsoft.AspNetCore.OutputCaching;

namespace FamilyBudgetTracker.Backend.API.Policy;

public class AuthenticatedUserCachePolicy : IOutputCachePolicy
{
    public static readonly AuthenticatedUserCachePolicy Instance = new();

    ValueTask IOutputCachePolicy.CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;
        context.ResponseExpirationTimeSpan = TimeSpan.FromMinutes(5);
        
        // Vary by any query by default
        // context.HttpContext.Request.Headers.TryGetValue("X-FamilyMember-ID", out var value);
        var value = context.HttpContext.User.Claims
            .First(x=> x.Type == AuthenticationConstants.ClaimTypes.ClaimUserIdType)
            .Value;
        
        context.CacheVaryByRules.QueryKeys = value;
        return ValueTask.CompletedTask;
    }

    // this never gets hit when Authorization is present
    ValueTask IOutputCachePolicy.ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var response = context.HttpContext.Response;
        context.AllowCacheStorage = true;

        return ValueTask.CompletedTask;
    }

    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        // Check if the current request fulfills the requirements to be cached

        var request = context.HttpContext.Request;

        // Verify the method
        if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method))
        {
            return false;
        }

        // Verify existence of authorization headers
        //if (!StringValues.IsNullOrEmpty(request.Headers.Authorization) || request.HttpContext.FamilyMember?.Identity?.IsAuthenticated == true)
        //{
        //    return false;
        //}
        return true;
    }
}