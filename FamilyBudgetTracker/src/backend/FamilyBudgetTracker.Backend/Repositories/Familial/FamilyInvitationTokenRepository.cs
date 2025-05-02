using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Repositories.Familial;

public class FamilyInvitationTokenRepository : IFamilyInvitationTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FamilyInvitationTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateInvitationToken(FamilyInvitationToken token)
    {
        await _dbContext.FamilyInvitationTokens.AddAsync(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteInvitationToken(FamilyInvitationToken token)
    {
        _dbContext.FamilyInvitationTokens.Remove(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FamilyInvitationToken?> GetInvitationToken(string id)
    {
        return await _dbContext.FamilyInvitationTokens.FirstOrDefaultAsync(t => t.Id == Guid.Parse(id));
    }
}