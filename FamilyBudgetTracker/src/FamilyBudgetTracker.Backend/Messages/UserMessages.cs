namespace FamilyBudgetTracker.Backend.Messages;

public static class UserMessages
{
    public static class ValidationMessages
    {
        public const string AlreadyRegistered = "User already registered.";

        public const string RegisterFailed = "Register failed";

        public const string UserNotFound = "User not found.";

        public const string InvalidEmailPassword = "Invalid email/password.";

        public const string UserNameRequired = "User name is required";

        public const string EmailRequired = "Email address is required";

        public const string ProvidedEmailIsNotEmail = "Invalid email format";

        public const string PasswordRequired = "Passsword is required";
        
        public const string PasswordMinLenght = "Passsword lenght must be atleast 3 characters";
        
        public const string PasswordLowerCaseRequirement = "Passsword must have atleast 1 lowercase characters";
        
        public const string PasswordUpperCaseRequirement = "Passsword must have atleast 1 uppercase characters";
        
        public const string PasswordNumberRequirement = "Passsword must have atleast 1 number";
        
        public const string PasswordSpecialCharacterRequirement = "Passsword must contain at least one (!? *.).";

        public const string AdminRequired = "Admin flag is required";

        public const string AccessTokenRequired = "Access token is required";

        public const string RefreshTokenRequired = "Refresh token is required";
    }

    public static class Messages
    {
        public const string AccountCreated = "Account created.";

        public const string LoginComplete = "Login complete.";

        public const string RefreshComplete = "Refresh complete";
    }
}