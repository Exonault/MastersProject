using FamilyBudgetTracker.Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetTracker.Backend.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);

    Task<User?> GetByName(string userName);

    Task<User?> GetById(string id);

    Task<User?> GetByRefreshToken(string refreshToken);

    Task<bool> CheckPassword(User user, string password);

    Task<IdentityResult> Create(User user, string password);

    Task AddToRole(User user, string role);

    Task<List<string>> GetAllRoles(User user);

    Task<string> GetMainFamilyRole(User user);

    Task UpdateUser(User user);
}