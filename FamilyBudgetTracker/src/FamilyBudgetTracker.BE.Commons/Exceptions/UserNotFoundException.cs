namespace FamilyBudgetTracker.BE.Commons.Exceptions;

public class UserNotFoundException:System.Exception
{
    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
    
}