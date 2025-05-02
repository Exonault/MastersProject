using System.Security.Claims;
using FamilyBudgetTracker.BE.Commons.Entities;

namespace FamilyBudgetTracker.BE.Commons.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user, List<string> roles);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetUserFromExpiredToken(string token);
}