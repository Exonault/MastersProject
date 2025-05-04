using System.Text.Json.Serialization;
using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Shared.Contracts.Familial.Family;

public class FamilyResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("familyMembers")]
    public List<UserResponse> FamilyMembers { get; set; }
}