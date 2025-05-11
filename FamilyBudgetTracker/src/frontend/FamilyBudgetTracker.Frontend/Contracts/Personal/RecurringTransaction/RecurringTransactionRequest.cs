namespace FamilyBudgetTracker.Frontend.Contracts.Personal.RecurringTransaction;

public class RecurringTransactionRequest
{
    public decimal Amount { get; set; }

    public string Description { get; set; }
    
    public string Type { get; set; }
    
    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }

    public int CategoryId { get; set; }
}