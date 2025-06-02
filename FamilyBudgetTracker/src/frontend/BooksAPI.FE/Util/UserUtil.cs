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

    public static string GetUserName(ClaimsPrincipal claimsPrincipal)
    {
        Claim? claim =
            claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "unique_name");

        if (claim is not null)
        {
            return claim.Value;
        }
        else return "";
    }    

    public static bool IsUserFamilyAdmin(ClaimsPrincipal claimsPrincipal)
    {
        List<Claim> userRoles =
            claimsPrincipal.Claims.Where(c => c.Type == ApplicationConstants.ClaimTypes.ClaimRoleType).ToList();


        Claim? familyAdmin = userRoles.FirstOrDefault(x => x.Value == ApplicationConstants.ClaimNames.FamilyAdminClaimName);

        return familyAdmin is not null;
    }
}