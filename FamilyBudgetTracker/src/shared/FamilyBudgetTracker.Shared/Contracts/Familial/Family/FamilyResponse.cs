using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.Shared.Contracts.Familial.Family;

public class FamilyResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}