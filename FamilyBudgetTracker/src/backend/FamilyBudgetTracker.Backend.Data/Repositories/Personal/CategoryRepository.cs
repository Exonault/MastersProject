using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data.Repositories.Personal;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;
 
    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCategory(PersonalCategory personalCategory)
    {
        await _dbContext.PersonalCategories.AddAsync(personalCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCategory(PersonalCategory personalCategory)
    {
        _dbContext.Entry(personalCategory).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategory(PersonalCategory personalCategory)
    {
        _dbContext.PersonalCategories.Remove(personalCategory);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PersonalCategory?> GetCategoryById(int id)
    {
        return await _dbContext.PersonalCategories
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<PersonalCategory>> GetAllCategoriesForUser(string userId)
    {
        return await _dbContext.PersonalCategories
            .Include(c => c.User)
            .Where(c => c.User.Id == userId)
            .ToListAsync();
    }
}