using System.Text.Json.Serialization;

namespace BooksAPI.FE.Contracts.User;

public class UserResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("userName")]
    public string UserName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; }
}