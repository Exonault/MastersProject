using System.Security.Claims;
using FamilyBudgetTracker.Backend.Constants;

namespace FamilyBudgetTracker.Backend.Util;

public static class UserUtil
{
    public static string GetUserIdFromAuth(this HttpContext httpContext)
    {
        ClaimsPrincipal user = httpContext.User;

        string userId = user.Claims
            .First(c => c.Type == ApplicationConstants.ClaimTypes.ClaimUserIdType)
            .Value;

        return userId;
    }

    public static string GetFamilyIdFromAuth(this HttpContext httpContext)
    {
        ClaimsPrincipal user = httpContext.User;

        string familyId = user.Claims
            .First(c => c.Type == ApplicationConstants.ClaimTypes.ClaimFamilyIdType)
            .Value;

        return familyId;
    }
}