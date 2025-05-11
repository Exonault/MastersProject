namespace FamilyBudgetTracker.FE.Interfaces;

public interface IRefreshTokenService
{
    Task<bool> RefreshToken(string token, string refreshToken);

    Task<string[]> GetTokens();
}