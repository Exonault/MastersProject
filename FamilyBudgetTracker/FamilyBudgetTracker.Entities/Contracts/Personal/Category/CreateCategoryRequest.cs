namespace FamilyBudgetTracker.Entities.Contracts.Personal.Category;

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Icon { get; set; } 

    public string Type { get; set; } = string.Empty;

    public decimal? Limit { get; set; } 

    public string UserId { get; set; } = string.Empty;
}