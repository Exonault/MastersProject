using System.Text.Json.Serialization;
using FamilyBudgetTracker.FE.Contracts.Familial.Family;

namespace FamilyBudgetTracker.FE.Contracts.Familial.FamilyCategory;

public class FamilyCategorySingleResponse
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
    public FamilyResponse FamilyResponse { get; set; }
}