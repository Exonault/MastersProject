using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.Shared.Contracts.Statistics;

public class CategoryStatistics
{
    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }

    [JsonPropertyName("categoryName")]
    public string CategoryName { get; set; }

    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }
}