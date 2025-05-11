using System.Security.Claims;
using FamilyBudgetTracker.Frontend.Constants;

namespace FamilyBudgetTracker.Frontend.Util;

public static class UserUtil
{
    public static string GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        Claim? claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AuthorizationConstants.ClaimTypes.ClaimUserIdType);

        if (claim is not null)
        {
            return claim.Value;
        }
        else return "";
    }
}