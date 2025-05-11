using FamilyBudgetTracker.Frontend.Contracts.User;
using FamilyBudgetTracker.Frontend.Models.User;

namespace FamilyBudgetTracker.Frontend.Interfaces;

public interface IUserService
{
    Task<RegisterResponse?> Register(RegisterModel model, bool isAdmin);
    
    Task<LoginResponse?> Login(LoginModel model);

    Task Logout(string token);
}