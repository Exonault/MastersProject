using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Backend.Authentication.Interfaces;

public interface IBearerUserService
{
    Task<RegisterResponse> RegisterAccount(RegisterRequest request);

    Task<LoginResponse> LoginAccount(LoginRequest request);

    Task<LoginResponse> Refresh(RefreshRequest request);

    Task Revoke(); 
}