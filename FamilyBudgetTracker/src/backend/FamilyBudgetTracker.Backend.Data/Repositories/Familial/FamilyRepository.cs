using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data.Repositories.Familial;

public class FamilyRepository : IFamilyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FamilyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateFamily(Family family)
    {
        await _dbContext.Family.AddAsync(family);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFamily(Family family)
    {
        _dbContext.Family.Remove(family);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Family?> GetFamilyById(string id)
    {
        return await _dbContext.Family
            // .Include(f => f.Categories)
            .Include(f => f.FamilyMembers)
            .FirstOrDefaultAsync(f => f.Id == Guid.Parse(id));
    }

    public async Task<List<Family>> GetAllFamilies()
    {
        return await _dbContext.Family
            .Include(f => f.Categories)
            .Include(f => f.FamilyMembers)
            .ToListAsync();
    }
}