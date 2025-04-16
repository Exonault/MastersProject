using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.Entities.Entities.Personal;
using FamilyBudgetTracker.Entities.Repositories.Personal;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Repositories.Personal;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;
 
    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCategory(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCategory(Category category)
    {
        _dbContext.Entry(category).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategory(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Category>> GetAllCategoriesForUser(string userId)
    {
        return await _dbContext.Categories
            .Include(c => c.User)
            .Where(c => c.User.Id == userId)
            .ToListAsync();
    }
}