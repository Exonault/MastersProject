using System.Security.Claims;
using FamilyBudgetTracker.Backend.Domain.Entities;

namespace FamilyBudgetTracker.Backend.Authentication.Token;

public interface IGenerateTokenService
{
    Task<string> GenerateAccessToken(User user);
    Task<string> GenerateAccessToken(string userId);

    string GenerateRefreshToken();
}