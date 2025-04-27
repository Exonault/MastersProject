using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;

namespace FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;

public class FamilyCategoryResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Icon { get; set; }

    public string Type { get; set; }

    public decimal? Limit { get; set; } //Monthly limit

    public FamilyResponse Family { get; set; }
}