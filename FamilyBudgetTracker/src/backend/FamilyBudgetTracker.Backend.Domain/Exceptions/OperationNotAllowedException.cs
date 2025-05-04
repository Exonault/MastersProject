namespace FamilyBudgetTracker.Backend.Domain.Exceptions;

public class OperationNotAllowedException : Exception
{
    public OperationNotAllowedException()
    {
        
    }
    
    public OperationNotAllowedException(string? message) : base(message)
    {
    }

    public OperationNotAllowedException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}