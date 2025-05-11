namespace BooksAPI.FE.Messages;

public static class UserMessages
{
    public const string UsernameErrorMessage = "Username must be at least 8 characters";

    public const string PasswordErrorMessage =
        "Password must be at least 8 characters and needs to contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.";

    public const string InvalidUserNameAndPassword = "Invalid username and/or password";

    public const string NoUser = "No user with this username";

    public const string ErrorLogging = "Login failed";

    public const string ErrorRegistering = "Register failed";

}