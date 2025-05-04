using FamilyBudgetTracker.Backend.Domain.Entities.Common;

namespace FamilyBudgetTracker.Backend.Domain.Entities.Personal;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Icon { get; set; }

    public CategoryType Type { get; set; } // Income, Expense

    public decimal? Limit { get; set; } //Monthly limit

    public User User { get; set; }

    public List<PersonalTransaction> Transactions { get; set; }

    public List<RecurringTransaction> RecurringTransactions { get; set; }
}