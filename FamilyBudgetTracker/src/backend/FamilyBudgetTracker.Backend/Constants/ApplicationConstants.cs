namespace FamilyBudgetTracker.Backend.Constants;

public static class ApplicationConstants
{
    public static class PolicyNames
    {
        public const string AdminRolePolicyName = "AdminPolicy";

        public const string UserRolePolicyName = "UserPolicy";

        public const string FamilyAdminPolicyName = "FamilyAdminPolicy";
        
        public const string FamilyMemberPolicyName = "FamilyMemberPolicy";
    }

    public static class ClaimNames
    {
        public const string AdminRoleClaimName = "Admin";

        public const string UserRoleClaimName = "User";

        public const string FamilyAdminClaimName = "FamilyAdmin";

        public const string FamilyMemberClaimName = "FamilyMember";
    }

    public static class ClaimTypes
    {
        public const string ClaimRoleType = "userRoles";

        public const string ClaimUserIdType = "userId";

        public const string ClaimFamilyIdType = "familyId";
    }

    public static class RoleTypes
    {
        public const string AdminRoleType = "Admin";

        public const string UserRoleType = "User";

        public const string FamilyAdminRoleType = "FamilyAdmin";

        public const string FamilyMemberRoleType = "FamilyMember";
    }

    public static class Cors
    {
        public const string CorsPolicy = "AllowedOrigin";
    }

    public static class Regex
    {
        public const string UpperCaseRegex = @"[A-Z]+";
        public const string LowerCaseRegex = @"[a-z]+";
        public const string NumberRegex = @"[0-1]+";
        public const string SpecialCharacterRegex = @"[\!\?\*\.]+";
    }

    public static class FamilyJoining
    {
        public const string JoinFamily = "Join family";
    }
}