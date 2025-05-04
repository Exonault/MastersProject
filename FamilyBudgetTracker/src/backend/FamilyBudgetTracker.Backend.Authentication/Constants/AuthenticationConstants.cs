namespace FamilyBudgetTracker.Backend.Authentication.Constants;

public static class AuthenticationConstants
{
    public static class ClaimTypes
    {
        public const string ClaimRoleType = "userRoles";

        public const string ClaimUserIdType = "userId";

        public const string ClaimFamilyIdType = "familyId";
    }

    public static class CookiesName
    {
        public const string AccessToken = "accessToken";

        public const string RefreshToken = "refreshToken";
    }
}