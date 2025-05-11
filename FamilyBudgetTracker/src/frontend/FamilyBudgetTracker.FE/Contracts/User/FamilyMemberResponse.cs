using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.FE.Contracts.User;

public class FamilyMemberResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("familyRole")]
    public string FamilyRole { get; set; }
}