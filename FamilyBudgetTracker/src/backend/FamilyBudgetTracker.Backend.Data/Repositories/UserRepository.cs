using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _applicationDbContext;

    public UserRepository(UserManager<User> userManager, ApplicationDbContext applicationDbContext)
    {
        _userManager = userManager;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User?> GetByName(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task<User?> GetById(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<User?> GetByRefreshToken(string refreshToken)
    {
        return await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> Create(User user, string password)
    {
        IdentityResult identityResult = await _userManager.CreateAsync(user, password);

        return identityResult;
    }

    public async Task AddToRole(User user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<List<string>> GetAllRoles(User user)
    {
        IList<string> roles = await _userManager.GetRolesAsync(user);

        return roles.ToList();
    }

    public async Task<string> GetMainFamilyRole(User user)
    {
        IList<string> roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains("FamilyAdmin"))
        {
            return "FamilyAdmin";
        }
        else if (roles.Contains("FamilyMember"))
        {
            return "FamilyMember";
        }
        else return "User";
    }

    public async Task UpdateUser(User user)
    {
        await _userManager.UpdateAsync(user);
    }
}