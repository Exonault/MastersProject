using System.Text.Json.Serialization;
using FamilyBudgetTracker.BE.Commons.Contracts.User;

namespace FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;

public class FamilyResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("familyMembers")]
    public List<UserResponse> FamilyMembers { get; set; }
}