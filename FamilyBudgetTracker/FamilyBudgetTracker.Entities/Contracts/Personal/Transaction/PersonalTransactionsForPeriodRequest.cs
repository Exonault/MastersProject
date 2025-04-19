namespace FamilyBudgetTracker.Entities.Contracts.Personal.Transaction;

public class PersonalTransactionsForPeriodRequest
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}