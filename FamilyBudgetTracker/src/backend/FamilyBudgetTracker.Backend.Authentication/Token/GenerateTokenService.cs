using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FamilyBudgetTracker.Backend.Authentication.Constants;
using FamilyBudgetTracker.Backend.Authentication.Messages;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Messages.User;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FamilyBudgetTracker.Backend.Authentication.Token;

public class GenerateTokenService : IGenerateTokenService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;

    public GenerateTokenService(IConfiguration config, IUserRepository userRepository)
    {
        _config = config;
        _userRepository = userRepository;
    }

    private static readonly TimeSpan TokenDuration = TimeSpan.FromHours(8); // test
    // private static readonly TimeSpan TokenDuration = TimeSpan.FromMinutes(5);
    
    public async Task<string> GenerateAccessToken(User user)
    {
        List<string> roles = await _userRepository.GetAllRoles(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        string secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException(AuthenticationMessages.SecretNotConfigured);
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(AuthenticationConstants.ClaimTypes.ClaimUserIdType, user.Id),
        ];

        if (user.Family is not null)
        {
            claims.Add(new Claim(AuthenticationConstants.ClaimTypes.ClaimFamilyIdType, user.Family.Id.ToString()));
        }

        foreach (string role in roles)
        {
            claims.Add(new Claim(AuthenticationConstants.ClaimTypes.ClaimRoleType, role));
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

    public async Task<string> GenerateAccessToken(string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }
        
        List<string> roles = await _userRepository.GetAllRoles(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        string secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException(AuthenticationMessages.SecretNotConfigured);
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(AuthenticationConstants.ClaimTypes.ClaimUserIdType, user.Id),
        ];

        if (user.Family is not null)
        {
            claims.Add(new Claim(AuthenticationConstants.ClaimTypes.ClaimFamilyIdType, user.Family.Id.ToString()));
        }

        foreach (string role in roles)
        {
            claims.Add(new Claim(AuthenticationConstants.ClaimTypes.ClaimRoleType, role));
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
}