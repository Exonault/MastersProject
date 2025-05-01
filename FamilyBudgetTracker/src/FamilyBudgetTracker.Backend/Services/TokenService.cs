using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.BE.Commons.Services;
using Microsoft.IdentityModel.Tokens;

namespace FamilyBudgetTracker.Backend.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
    }

    private static readonly TimeSpan TokenDuration = TimeSpan.FromHours(8); // test
    // private static readonly TimeSpan TokenDuration = TimeSpan.FromMinutes(5);

    
    public string GenerateAccessToken()
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        byte[] randomNumbers = new byte[64];

        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(randomNumbers);

        string token = Convert.ToBase64String(randomNumbers);

        return token;
    }

    public ClaimsPrincipal? GetUserFromExpiredToken(string token)
    {
        string secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException(ApplicationMessages.SecretNotConfigured);

        TokenValidationParameters validation = new TokenValidationParameters()
        {
            ValidIssuer = _config["Jwt:Issuer"],
            ValidAudience = _config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateLifetime = false,
        };

        ClaimsPrincipal claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);

        return claimsPrincipal;
    }
}