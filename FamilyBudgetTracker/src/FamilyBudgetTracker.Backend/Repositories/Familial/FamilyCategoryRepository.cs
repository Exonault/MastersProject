using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Repositories.Familial;

public class FamilyCategoryRepository : IFamilyCategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FamilyCategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task CreateFamilyCategory(FamilyCategory category)
    {
        await _dbContext.FamilyCategories.AddRangeAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateFamilyCategory(FamilyCategory category)
    {
        _dbContext.Entry(category).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFamilyCategory(FamilyCategory category)
    {
        _dbContext.FamilyCategories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FamilyCategory?> GetCategoryById(int id)
    {
        return await _dbContext.FamilyCategories
            .Include(fc => fc.Family)
            .FirstOrDefaultAsync(fc => fc.Id == id);
    }

    public async Task<List<FamilyCategory>> GetCategoriesByFamilyId(int familyId)
    {
        return await _dbContext.FamilyCategories
            .Include(fc => fc.Family)
            .Where(fc => fc.Family.Id == familyId)
            .ToListAsync();
    }
}