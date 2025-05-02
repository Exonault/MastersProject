namespace FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;

public class CreateFamilyCategoryRequest
{
    public string Name { get; set; }

    public string? Icon { get; set; }

    public string Type { get; set; }

    public decimal? Limit { get; set; } //Monthly limit
}