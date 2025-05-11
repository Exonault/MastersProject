namespace FamilyBudgetTracker.FE.Contracts.Personal.Transaction;

public class PersonalTransactionRequest
{
    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateOnly TransactionDate { get; set; }

    public int CategoryId { get; set; }
}