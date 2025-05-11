using System.Security.Claims;
using BooksAPI.FE.Constants;

namespace BooksAPI.FE.Util;

public static class UserUtil
{
    public static string GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        Claim? claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ApplicationConstants.ClaimTypes.ClaimUserIdType);

        if (claim is not null)
        {
            return claim.Value;
        }
        else return "";

    }
    
}