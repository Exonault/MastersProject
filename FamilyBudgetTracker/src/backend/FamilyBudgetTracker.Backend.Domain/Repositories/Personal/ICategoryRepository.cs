using FamilyBudgetTracker.Backend.Domain.Entities.Personal;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Personal;

public interface ICategoryRepository
{
    Task CreateCategory(Category category);

    Task UpdateCategory(Category category);

    Task DeleteCategory(Category category);

    Task<Category?> GetCategoryById(int id);

    Task<List<Category>> GetAllCategoriesForUser(string userId);
    
}