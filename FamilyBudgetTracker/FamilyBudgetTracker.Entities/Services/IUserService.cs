using FamilyBudgetTracker.Entities.Contracts.User;

namespace FamilyBudgetTracker.Entities.Services;

public interface IUserService
{
    Task<RegisterResponse> RegisterAccount(RegisterRequest request);

    Task<LoginResponse> LoginAccount(LoginRequest request);

    Task<LoginResponse> Refresh(RefreshRequest request);

    Task Revoke(); 
}