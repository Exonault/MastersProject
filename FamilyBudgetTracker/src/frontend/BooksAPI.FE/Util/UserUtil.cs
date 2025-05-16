using System.Security.Claims;
using BooksAPI.FE.Constants;

namespace BooksAPI.FE.Util;

public static class UserUtil
{
    public static string GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        Claim? claim =
            claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ApplicationConstants.ClaimTypes.ClaimUserIdType);

        if (claim is not null)
        {
            return claim.Value;
        }
        else return "";
    }

    public static string GetFamilyId(ClaimsPrincipal claimsPrincipal)
    {
        Claim? claim =
            claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ApplicationConstants.ClaimTypes.ClaimFamilyIdType);

        if (claim is not null)
        {
            return claim.Value;
        }
        else return "";
    }
}