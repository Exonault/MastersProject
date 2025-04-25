using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;

namespace FamilyBudgetTracker.BE.Commons.Contracts.Personal.RecurringTransaction;

public class RecurringTransactionResponse
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }
    
    public string Type { get; set; }
    
    public DateOnly StartDate { get; set; }

    public DateOnly NextExecutionDate { get; set; } // Calculated on execution in cronjob?

    public DateOnly EndDate { get; set; }

    public CategoryResponse Category { get; set; }
}