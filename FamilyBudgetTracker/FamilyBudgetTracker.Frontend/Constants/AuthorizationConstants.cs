namespace FamilyBudgetTracker.Frontend.Constants;

public class AuthorizationConstants
{
    public static class PolicyNames
    {
        public const string AdminRolePolicyName = "AdminPolicy";

        public const string UserRolePolicyName = "UserPolicy";
    }

    public static class ClaimNames
    {
        public const string AdminRoleClaimName = "Admin";

        public const string UserRoleClaimName = "User";
    }

    public static class ClaimTypes
    {
        public const string ClaimRoleType = "userRoles";

        public const string ClaimUserIdType = "userId";
    }

    public static class ClaimValues
    {
        public const string AdminRoleClaimName = "Admin";

        public const string UserRoleClaimName = "User";

        public const string FamilyAdminClaimName = "FamilyAdmin";

        public const string FamilyMemberClaimName = "FamilyMember";
    }

    public static class RoleTypes
    {
        public const string AdminRoleType = "Admin";

        public const string UserRoleType = "User";
    }
}