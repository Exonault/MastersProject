namespace FamilyBudgetTracker.Backend.Data.DTO.User;

public class RefreshDto
{
    public string AccessToken { get; set; } = string.Empty;
    
    public string RefreshToken { get; set; } = string.Empty;
}