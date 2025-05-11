namespace BooksAPI.FE.Contracts.Familial.FamilyTransaction;

public class FamilyTransactionRequest
{
    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateOnly TransactionDate { get; set; }

    public int FamilyCategoryId { get; set; }
}