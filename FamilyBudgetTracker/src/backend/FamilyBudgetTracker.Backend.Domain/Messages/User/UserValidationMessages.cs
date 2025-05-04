namespace FamilyBudgetTracker.Backend.Domain.Messages.User;

public static class UserValidationMessages
{
    public const string UserNotFound = "User not found.";
    
    public const string NoFamilyForUser = "User does not have a family";

    public const string UserIsNotFromFamily = "User is not from this family";
    
    public const string AlreadyRegistered = "User already registered.";
    
    public const string RegisterFailed = "Register failed";
    
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