using FamilyBudgetTracker.Frontend.Models.User;
using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Frontend.Interfaces;

public interface IUserService
{
    Task<LoginResponse?> Login(LoginModel model);
}