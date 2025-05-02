using FamilyBudgetTracker.BE.Commons.Contracts.User;

namespace FamilyBudgetTracker.BE.Commons.Services.Auth;

public interface IUserService
{
    Task<RegisterResponse> RegisterAccount(RegisterRequest request);

    Task<LoginResponse> LoginAccount(LoginRequest request);

    Task<LoginResponse> Refresh(RefreshRequest request);

    Task Revoke();

    Task<bool> AddUserToFamily(string tokenId, string familyId);
}