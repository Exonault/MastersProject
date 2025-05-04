using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.Shared.Contracts.Personal.Category;

public class CategoryResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("limit")]
    public decimal? Limit { get; set; }

    // [JsonPropertyName("userId")]
    // public string UserId { get; set; } = string.Empty;
}