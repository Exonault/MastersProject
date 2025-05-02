using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Repositories.Familial;

public class FamilyVerificationTokenRepository : IFamilyVerificationTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FamilyVerificationTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateVerificationToken(FamilyVerificationToken token)
    {
        await _dbContext.FamilyVerificationTokens.AddAsync(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteVerificationToken(FamilyVerificationToken token)
    {
        _dbContext.FamilyVerificationTokens.Remove(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FamilyVerificationToken?> GetVerificationToken(string id)
    {
        return await _dbContext.FamilyVerificationTokens.FirstOrDefaultAsync(t => t.Id == Guid.Parse(id));
    }
}