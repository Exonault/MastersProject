namespace FamilyBudgetTracker.Backend.Domain.Entities.Personal;

public class PersonalTransaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateOnly TransactionDate { get; set; }

    public Category Category { get; set; }

    public User User { get; set; }
}