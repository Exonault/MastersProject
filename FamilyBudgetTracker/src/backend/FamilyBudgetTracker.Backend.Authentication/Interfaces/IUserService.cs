using FamilyBudgetTracker.Shared.Contracts.User;
using Microsoft.AspNetCore.Http;

namespace FamilyBudgetTracker.Backend.Authentication.Interfaces;

public interface IUserService
{
    Task<RegisterResponse> RegisterAccount(RegisterRequest request);
    
    Task LoginAccount(LoginRequest request, HttpContext httpContext);
    
    Task Refresh(string? refreshToken , HttpContext httpContext);

    Task Revoke(HttpContext httpContext);

    Task UpdateUserAccessToken(HttpContext httpContext);
}