using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetTracker.Frontend.Models.Category;

public class CategoryModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
    
    public string? Icon { get; set; }
    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; } = string.Empty;
    
    public decimal? Limit { get; set; }
}