namespace FamilyBudgetTracker.Entities.Exceptions;

public class InvalidEmailPasswordException: System.Exception
{
    public InvalidEmailPasswordException()
    {
    }

    public InvalidEmailPasswordException(string? message) : base(message)
    {
    }

    public InvalidEmailPasswordException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
    
}