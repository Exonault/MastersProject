namespace FamilyBudgetTracker.BE.Commons.Contracts.Personal.Transaction;

public class PersonalTransactionsForPeriodRequest
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
}