using FamilyBudgetTracker.Backend.Domain.Entities.Personal;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Personal;

public interface ICategoryRepository
{
    Task CreateCategory(PersonalCategory personalCategory);

    Task UpdateCategory(PersonalCategory personalCategory);

    Task DeleteCategory(PersonalCategory personalCategory);

    Task<PersonalCategory?> GetCategoryById(int id);

    Task<List<PersonalCategory>> GetAllCategoriesForUser(string userId);
    
}