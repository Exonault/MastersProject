namespace FamilyBudgetTracker.Shared.Contracts.Personal.Transaction;

public class CreatePersonalTransactionRequest
{
    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateOnly TransactionDate { get; set; }

    public int CategoryId { get; set; }
}