using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.BE.Commons.Entities;
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


    public string GenerateAccessToken(User user, List<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        string secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException(ApplicationMessages.SecretNotConfigured);
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ApplicationConstants.ClaimTypes.ClaimUserIdType, user.Id),
        ];

        if (user.Family is not null)
        {
            claims.Add(new Claim(ApplicationConstants.ClaimTypes.ClaimFamilyIdType, user.Family.Id.ToString()));
        }

        foreach (string role in roles)
        {
            claims.Add(new Claim(ApplicationConstants.ClaimTypes.ClaimRoleType, role));
        }

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenDuration),
            Issuer = _config["Jwt:Issuer"],
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        string jwt = tokenHandler.WriteToken(token);

        return jwt;
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