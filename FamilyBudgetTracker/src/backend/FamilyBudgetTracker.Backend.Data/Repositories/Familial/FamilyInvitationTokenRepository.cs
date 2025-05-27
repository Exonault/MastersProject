using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data.Repositories.Familial;

public class FamilyInvitationTokenRepository : IFamilyInvitationTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FamilyInvitationTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateInvitationToken(FamilyInvitations token)
    {
        await _dbContext.FamilyInvitations.AddAsync(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteInvitationToken(FamilyInvitations token)
    {
        _dbContext.FamilyInvitations.Remove(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FamilyInvitations?> GetInvitationToken(string id)
    {
        return await _dbContext.FamilyInvitations.FirstOrDefaultAsync(t => t.Id == Guid.Parse(id));
    }
}