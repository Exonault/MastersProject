using System.Security.Claims;

namespace FamilyBudgetTracker.BE.Commons.Services;

public interface ITokenService
{
    string GenerateAccessToken();

    string GenerateRefreshToken();

    ClaimsPrincipal? GetUserFromExpiredToken(string token);
}