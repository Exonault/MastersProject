namespace FamilyBudgetTracker.Backend.Messages;

public static class UserMessages
{
    public static class ValidationMessages
    {
        public const string EmptyRequest = "Empty request.";

        public const string AlreadyRegistered = "User already registered.";

        public const string RegisterFailed = "Register failed";

        public const string ErrorOccured = "Error occured.";

        public const string UserNotFound = "User not found.";

        public const string InvalidUsernamePassword = "Invalid username/password.";
    }

    public static class Messages
    {
        public const string AccountCreated = "Account created.";

        public const string LoginComplete = "Login complete.";

        public const string RefreshComplete = "Refresh complete";
    }
}