using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.Frontend.Contracts.Familial.Family;

public class FamilyResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}