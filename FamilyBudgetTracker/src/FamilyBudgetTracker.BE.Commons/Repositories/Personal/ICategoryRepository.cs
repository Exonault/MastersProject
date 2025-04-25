using FamilyBudgetTracker.BE.Commons.Entities.Personal;

namespace FamilyBudgetTracker.BE.Commons.Repositories.Personal;

public interface ICategoryRepository
{
    Task CreateCategory(Category category);

    Task UpdateCategory(Category category);

    Task DeleteCategory(Category category);

    Task<Category?> GetCategoryById(int id);

    Task<List<Category>> GetAllCategoriesForUser(string userId);
    
}