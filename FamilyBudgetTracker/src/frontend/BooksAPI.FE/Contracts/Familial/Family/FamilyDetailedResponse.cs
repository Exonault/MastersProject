using System.Text.Json.Serialization;
using BooksAPI.FE.Contracts.User;

namespace BooksAPI.FE.Contracts.Familial.Family;

public class FamilyDetailedResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("familyMembers")]
    public List<FamilyMemberResponse> FamilyMembers { get; set; }
}