namespace BooksAPI.FE.Contracts.User;

public class RefreshRequest
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}