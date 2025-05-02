using System.Security.Claims;
using FamilyBudgetTracker.BE.Commons.Entities;

namespace FamilyBudgetTracker.BE.Commons.Services.Auth;

public interface IGenerateTokenService
{
    Task<string> GenerateAccessToken(User user);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetUserFromExpiredToken(string token);
}