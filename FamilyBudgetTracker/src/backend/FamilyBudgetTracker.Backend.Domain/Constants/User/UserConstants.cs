namespace FamilyBudgetTracker.Backend.Domain.Constants.User;

public static class UserConstants
{
    public static class RoleTypes
    {
        public const string AdminRoleType = "Admin";

        public const string UserRoleType = "FamilyMember";

        public const string FamilyAdminRoleType = "FamilyAdmin";

        public const string FamilyMemberRoleType = "FamilyMember";
    }
    
    public static class RoleId
    {
        public const string AdminRoleId = "admin";

        public const string UserRoleId = "user";

        public const string FamilyAdminRoleId = "familyAdmin";

        public const string FamilyMemberRoleId = "familyMember";
    }
    
    public static class RoleNormalizedNames
    {
        public const string Admin = "ADMIN";

        public const string User= "USER";

        public const string FamilyAdmin = "FAMILYADMIN";

        public const string FamilyMember = "FAMILYMEMBER";
    }
}