namespace FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Icon { get; set; } 

    public string Type { get; set; } = string.Empty;

    public decimal? Limit { get; set; } 
}