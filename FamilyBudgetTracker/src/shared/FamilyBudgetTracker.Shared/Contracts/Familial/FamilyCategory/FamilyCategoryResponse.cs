using System.Text.Json.Serialization;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;

namespace FamilyBudgetTracker.Shared.Contracts.Familial.FamilyCategory;

public class FamilyCategoryResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("Type")]
    public string Type { get; set; }

    [JsonPropertyName("limit")]
    public decimal? Limit { get; set; } //Monthly limit

    [JsonPropertyName("family")]
    public FamilyResponse Family { get; set; }
}