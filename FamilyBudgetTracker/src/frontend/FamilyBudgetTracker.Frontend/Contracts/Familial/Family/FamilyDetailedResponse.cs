using System.Text.Json.Serialization;
using FamilyBudgetTracker.Frontend.Contracts.User;

namespace FamilyBudgetTracker.Frontend.Contracts.Familial.Family;

public class FamilyDetailedResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("familyMembers")]
    public List<FamilyMemberResponse> FamilyMembers { get; set; }
}