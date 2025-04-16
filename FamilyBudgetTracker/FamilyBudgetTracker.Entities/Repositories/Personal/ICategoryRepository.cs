using FamilyBudgetTracker.Entities.Entities.Personal;

namespace FamilyBudgetTracker.Entities.Repositories.Personal;

public interface ICategoryRepository
{
    Task CreateCategory(Category category);

    Task UpdateCategory(Category category);

    Task DeleteCategory(Category category);

    Task<Category?> GetCategoryById(int id);

    Task<List<Category>> GetAllCategoriesForUser(string userId);
    
}