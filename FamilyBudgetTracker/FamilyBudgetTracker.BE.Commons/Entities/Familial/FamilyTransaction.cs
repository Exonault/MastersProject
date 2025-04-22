namespace FamilyBudgetTracker.BE.Commons.Entities.Familial;

public class FamilyTransaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public DateOnly TransactionDate { get; set; }

    public FamilyCategory Category { get; set; }
    
    public User User { get; set; }

    public Family Family { get; set; }
}