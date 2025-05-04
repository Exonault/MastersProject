using System.Security.Claims;
using FamilyBudgetTracker.Backend.Authentication.Constants;
using Microsoft.AspNetCore.Http;

namespace FamilyBudgetTracker.Backend.Authentication.Util;

public static class UserUtil
{
    public static string GetUserIdFromAuth(this HttpContext httpContext)
    {
        ClaimsPrincipal user = httpContext.User;

        string userId = user.Claims
            .First(c => c.Type == AuthenticationConstants.ClaimTypes.ClaimUserIdType)
            .Value;

        return userId;
    }

    public static string GetFamilyIdFromAuth(this HttpContext httpContext)
    {
        ClaimsPrincipal user = httpContext.User;

        string familyId = user.Claims
            .First(c => c.Type == AuthenticationConstants.ClaimTypes.ClaimFamilyIdType)
            .Value;

        return familyId;
    }
}