using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.Entities.Contracts.User;

public class RegisterResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("successful")]
    public bool Successful { get; set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; set; } = new List<string>();
}