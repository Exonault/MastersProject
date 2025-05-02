using System.Security.Claims;
using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Mappers;
using FamilyBudgetTracker.BE.Commons.Messages;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using FamilyBudgetTracker.BE.Commons.Services.Auth;
using FamilyBudgetTracker.BE.Commons.Validation;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetTracker.Backend.Services.Auth;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenerateTokenService _generateTokenService;
    private readonly IFamilyInvitationTokenRepository _familyInvitationTokenRepository;
    private readonly IFamilyRepository _familyRepository;

    public UserService(IUserRepository repository, IHttpContextAccessor httpContextAccessor, IGenerateTokenService generateTokenService,
        IFamilyInvitationTokenRepository familyInvitationTokenRepository, IFamilyRepository familyRepository)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _generateTokenService = generateTokenService;
        _familyInvitationTokenRepository = familyInvitationTokenRepository;
        _familyRepository = familyRepository;
    }

    public async Task<RegisterResponse> RegisterAccount(RegisterRequest request)
    {
        User? userByEmail = await _repository.GetByEmail(request.Email);

        if (userByEmail is not null)
        {
            throw new UserAlreadyRegisteredException(UserMessages.ValidationMessages.AlreadyRegistered);
        }

        User user = request.ToUser();

        IdentityResult identityResult = await _repository.Create(user, request.Password);

        if (!identityResult.Succeeded)
        {
            List<string> errors = identityResult.Errors.Select(e => e.Description).ToList();

            RegisterResponse failedResponse = new RegisterResponse
            {
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

        string token = await _generateTokenService.GenerateAccessToken(user);
        string refreshToken = _generateTokenService.GenerateRefreshToken();

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
        ClaimsPrincipal? principal = _generateTokenService.GetUserFromExpiredToken(request.AccessToken);

        if (principal?.Identity?.Name is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        User? user = await _repository.GetByName(principal.Identity.Name);

        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        List<string> roles = await _repository.GetAllRoles(user);

        string token = await _generateTokenService.GenerateAccessToken(user);

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

    //TODO refactor and/or extract into another service ?
    public async Task<bool> AddUserToFamily(string tokenId, string familyId)
    {
        var token = await _familyInvitationTokenRepository.GetInvitationToken(tokenId);

        if (token is null || token.ExpiresOnUtc < DateTime.UtcNow)
        {
            return false;
        }

        if (!token.UserInApplication)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        User? user = await _repository.GetByEmail(token.Email);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(token.FamilyId);

        family = family.ValidateFamily();

        user.Family = family;

        await _repository.AddToRole(user, ApplicationConstants.RoleTypes.FamilyMemberRoleType);

        await _repository.UpdateUser(user);

        await _familyInvitationTokenRepository.DeleteInvitationToken(token);

        throw new NotImplementedException();
    }
}