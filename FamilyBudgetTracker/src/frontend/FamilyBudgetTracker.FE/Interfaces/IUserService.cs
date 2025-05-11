using FamilyBudgetTracker.FE.Contracts.User;
using FamilyBudgetTracker.FE.Model;

namespace FamilyBudgetTracker.FE.Interfaces;

public interface IUserService
{
    Task<RegisterResponse?> Register(RegisterModel model, bool isAdmin);

    Task<LoginResponse?> Login(LoginModel model);

    Task Logout(string token);
}