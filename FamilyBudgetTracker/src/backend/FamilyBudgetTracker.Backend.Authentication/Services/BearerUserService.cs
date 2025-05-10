using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FamilyBudgetTracker.Backend.Authentication.Interfaces;
using FamilyBudgetTracker.Backend.Authentication.Token;
using FamilyBudgetTracker.Backend.Data.Mappers;
using FamilyBudgetTracker.Backend.Domain.Constants.User;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Messages.User;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Shared.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FamilyBudgetTracker.Backend.Authentication.Services;

public class BearerUserService : IBearerUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IGenerateTokenService _generateTokenService;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public BearerUserService(IConfiguration config, IGenerateTokenService generateTokenService, IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _generateTokenService = generateTokenService;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<RegisterResponse> RegisterAccount(RegisterRequest request)
    {
        User? userByEmail = await _userRepository.GetByEmail(request.Email);

        if (userByEmail is not null)
        {
            throw new UserAlreadyRegisteredException(UserValidationMessages.AlreadyRegistered);
        }

        User user = request.ToUser();

        IdentityResult identityResult = await _userRepository.Create(user, request.Password);

        if (!identityResult.Succeeded)
        {
            List<string> errors = identityResult.Errors.Select(e => e.Description).ToList();

            RegisterResponse failedResponse = new RegisterResponse
            {
                Message = UserValidationMessages.RegisterFailed,
                Errors = errors,
                Successful = false,
            };

            return failedResponse;
        }

        if (request.Admin)
        {
            await _userRepository.AddToRole(user, UserConstants.RoleTypes.AdminRoleType);
        }

        await _userRepository.AddToRole(user, UserConstants.RoleTypes.UserRoleType);

        RegisterResponse response = new RegisterResponse
        {
            Message = UserMessages.AccountCreated,
            Successful = true,
            Errors = new List<string>(),
        };
        return response;
    }

    public async Task<LoginResponse> LoginAccount(LoginRequest request)
    {
        User? user = await _userRepository.GetByEmail(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        bool checkPassword = await _userRepository.CheckPassword(user, request.Password);

        if (!checkPassword)
        {
            throw new InvalidEmailPasswordException(UserValidationMessages.InvalidEmailPassword);
        }

        string accessToken = await _generateTokenService.GenerateAccessToken(user);
        string refreshToken = _generateTokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

        await _userRepository.UpdateUser(user);

        LoginResponse response = new LoginResponse
        {
            Message = UserMessages.LoginComplete,
            Token = accessToken,
            RefreshToken = refreshToken
        };

        return response;
    }

    public async Task<LoginResponse> Refresh(RefreshRequest request)
    {
        ClaimsPrincipal? principal = GetUserFromExpiredToken(request.AccessToken);

        if (principal?.Identity?.Name is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        User? user = await _userRepository.GetByName(principal.Identity.Name);

        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        string token = await _generateTokenService.GenerateAccessToken(user);

        LoginResponse response = new LoginResponse
        {
            Token = token,
            RefreshToken = request.RefreshToken,
            Message = UserMessages.RefreshComplete,
        };
        return response;
    }

    public async Task Revoke()
    {
        string? userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

        if (userName is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        User? user = await _userRepository.GetByName(userName);

        if (user is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        user.RefreshToken = null;

        await _userRepository.UpdateUser(user);
    }

    private ClaimsPrincipal? GetUserFromExpiredToken(string token)
    {
        string secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException("Secret not configured");

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