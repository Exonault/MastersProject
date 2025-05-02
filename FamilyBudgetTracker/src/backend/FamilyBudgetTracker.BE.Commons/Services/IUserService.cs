using FamilyBudgetTracker.BE.Commons.Contracts.User;

namespace FamilyBudgetTracker.BE.Commons.Services;

public interface IUserService
{
    Task<RegisterResponse> RegisterAccount(RegisterRequest request);

    Task<LoginResponse> LoginAccount(LoginRequest request);

    Task<LoginResponse> Refresh(RefreshRequest request);

    Task Revoke(); 
}