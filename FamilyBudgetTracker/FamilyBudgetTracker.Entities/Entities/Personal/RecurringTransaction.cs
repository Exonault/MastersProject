using FamilyBudgetTracker.Entities.Entities.Common;

namespace FamilyBudgetTracker.Entities.Entities.Personal;

public class RecurringTransaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }
    
    public DateOnly StartDate { get; set; }
    
    public DateOnly NextExecutionDate { get; set; } // Calculated on execution in cronjob?
    
    public DateOnly EndDate { get; set; }

    public Category Category { get; set; }

    public User User { get; set; }
}