namespace FamilyBudgetTracker.Backend.Domain.Exceptions;

public class MappingException : Exception
{
    public MappingException()
    {
        
    }
    
    public MappingException(string? message) : base(message)
    {
    }

    public MappingException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}