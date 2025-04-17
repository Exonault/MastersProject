using FamilyBudgetTracker.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace FamilyBudgetTracker.Entities.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);

    Task<User?> GetByName(string userName);

    Task<User?> GetById(string id);

    Task<bool> CheckPassword(User user, string password);

    Task<IdentityResult> Create(User user, string password);

    Task AddToRole(User user, string role);

    Task<List<string>> GetAllRoles(User user);

    Task UpdateUser(User user);
}