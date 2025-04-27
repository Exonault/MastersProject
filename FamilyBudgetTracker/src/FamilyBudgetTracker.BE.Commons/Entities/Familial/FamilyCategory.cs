using FamilyBudgetTracker.BE.Commons.Entities.Common;

namespace FamilyBudgetTracker.BE.Commons.Entities.Familial;

public class FamilyCategory
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Icon { get; set; }

    public CategoryType Type { get; set; }

    public decimal? Limit { get; set; } //Monthly limit

    public Family Family { get; set; }

    public List<FamilyTransaction> FamilyTransactions { get; set; }
}