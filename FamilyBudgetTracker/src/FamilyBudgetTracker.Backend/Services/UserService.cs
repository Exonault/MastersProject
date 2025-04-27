using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Mappers;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FamilyBudgetTracker.Backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private static readonly TimeSpan TokenDuration = TimeSpan.FromHours(8); // test
    // private static readonly TimeSpan TokenDuration = TimeSpan.FromMinutes(5);

    public UserService(IUserRepository repository, IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

     public async Task<RegisterResponse> RegisterAccount(RegisterRequest request)
    {
         User? userByEmail = await _repository.GetByEmail(request.Email);

         if (userByEmail is not null)
         {
             throw new UserAlreadyRegisteredException(UserMessages.ValidationMessages.AlreadyRegistered);
         }

         User user = request.ToUser();
         
         // User user = new User()
         // {
         //     UserName = request.UserName,
         //     PasswordHash = request.Password,
         //     Email = request.Email
         // };

         IdentityResult identityResult = await _repository.Create(user, request.Password);

         if (!identityResult.Succeeded)
         {
             List<string> errors = identityResult.Errors.Select(e => e.Description).ToList();
             
             RegisterResponse failedResponse = new RegisterResponse{
                 Message = UserMessages.ValidationMessages.RegisterFailed, 
                 Errors = errors,
                 Successful = false,
             };
             
             return failedResponse;
         }

         if (request.Admin)
         {
             await _repository.AddToRole(user, ApplicationConstants.RoleTypes.AdminRoleType);
         }

         await _repository.AddToRole(user, ApplicationConstants.RoleTypes.UserRoleType);

         RegisterResponse response = new RegisterResponse
         {
             Message = UserMessages.Messages.AccountCreated,
             Successful = true,
             Errors = new List<string>(),
         };
         return response;
    }

    public async Task<LoginResponse> LoginAccount(LoginRequest request)
    {
        User? user = await _repository.GetByEmail(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        bool checkPassword = await _repository.CheckPassword(user, request.Password);

        if (!checkPassword)
        {
            throw new InvalidEmailPasswordException(UserMessages.ValidationMessages.InvalidEmailPassword);
        }

        List<string> roles = await _repository.GetAllRoles(user);

        string token = GenerateToken(user.UserName!, user.Id, roles);

        string refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

        await _repository.UpdateUser(user);

        LoginResponse response = new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Message = UserMessages.Messages.LoginComplete,
        };
        return response;
    }

    public async Task<LoginResponse> Refresh(RefreshRequest request)
    {
        ClaimsPrincipal? principal = GetUserFromExpiredToken(request.AccessToken);

        if (principal?.Identity?.Name is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        User? user = await _repository.GetByName(principal.Identity.Name);

        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        string token = GenerateToken(user.UserName!, user.Id, await _repository.GetAllRoles(user));

        LoginResponse response = new LoginResponse
        {
            Token = token,
            RefreshToken = request.RefreshToken,
            Message = UserMessages.Messages.RefreshComplete,
        };
        return response;
    }

    public async Task Revoke()
    {
        string? userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

        if (userName is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        User? user = await _repository.GetByName(userName);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        user.RefreshToken = null;

        await _repository.UpdateUser(user);
    }

    private string GenerateToken(string userName, string id, List<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        string secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException(ApplicationMessages.SecretNotConfigured);
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ApplicationConstants.ClaimTypes.ClaimUserIdType, id),
        ];

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


    private string GenerateRefreshToken()
    {
        byte[] randomNumbers = new byte[64];

        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(randomNumbers);

        string token = Convert.ToBase64String(randomNumbers);

        return token;
    }

    private ClaimsPrincipal? GetUserFromExpiredToken(string token)
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