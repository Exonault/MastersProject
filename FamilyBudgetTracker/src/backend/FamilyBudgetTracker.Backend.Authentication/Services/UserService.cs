using FamilyBudgetTracker.Backend.Authentication.Interfaces;
using FamilyBudgetTracker.Backend.Authentication.Messages;
using FamilyBudgetTracker.Backend.Authentication.Token;
using FamilyBudgetTracker.Backend.Data.Mappers;
using FamilyBudgetTracker.Backend.Domain.Constants.User;
using FamilyBudgetTracker.Backend.Domain.DTO.User;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Exceptions;
using FamilyBudgetTracker.Backend.Domain.Messages.User;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Shared.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetTracker.Backend.Authentication.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IGenerateTokenService _generateTokenService;
    private readonly IApplicationAuthenticationService _applicationAuthenticationService;


    public UserService(IUserRepository userRepository, IGenerateTokenService generateTokenService,
        IApplicationAuthenticationService applicationAuthenticationService)
    {
        _userRepository = userRepository;
        _generateTokenService = generateTokenService;
        _applicationAuthenticationService = applicationAuthenticationService;
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

    public async Task LoginAccount(LoginRequest request, HttpContext httpContext)
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

        TokenDto tokenDto = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        _applicationAuthenticationService.SetTokensInsideCookie(tokenDto, httpContext);
    }

    public async Task Refresh(string? refreshToken, HttpContext httpContext)
    {
        if (refreshToken is null)
        {
            throw new OperationNotAllowedException(AuthenticationMessages.RefreshTokenNotProvided);
        }

        User? user = await _userRepository.GetByRefreshToken(refreshToken);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        string newAccessToken = await _generateTokenService.GenerateAccessToken(user);

        TokenDto tokenDto = new TokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = refreshToken
        };

        _applicationAuthenticationService.SetTokensInsideCookie(tokenDto, httpContext);
    }

    public async Task Revoke(HttpContext httpContext)
    {
        string? userName = httpContext.User.Identity?.Name;

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

        _applicationAuthenticationService.RemoveTokensInsideCookie(httpContext);
    }

    public async Task UpdateUserAccessToken(HttpContext httpContext)
    {
        string? userName = httpContext.User.Identity?.Name;

        if (userName is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        User? user = await _userRepository.GetByName(userName);

        if (user is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        string newAccessToken = await _generateTokenService.GenerateAccessToken(user);

        httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

        if (refreshToken is null)
        {
            throw new OperationNotAllowedException(AuthenticationMessages.RefreshTokenNotProvided);
        }

        TokenDto updatedTokens = new TokenDto
        {
            AccessToken = newAccessToken,
            RefreshToken = refreshToken
        };

        _applicationAuthenticationService.SetTokensInsideCookie(updatedTokens, httpContext);
    }

    public async Task<UserResponse> GetUserInformation(HttpContext httpContext)
    {
        string? userName = httpContext.User.Identity?.Name;

        if (userName is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        User? user = await _userRepository.GetByName(userName);

        if (user is null)
        {
            throw new UserNotFoundException(UserValidationMessages.UserNotFound);
        }

        List<string> roles = await _userRepository.GetAllRoles(user);

        UserResponse userResponse = new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Roles = roles
        };

        return userResponse;
    }
}