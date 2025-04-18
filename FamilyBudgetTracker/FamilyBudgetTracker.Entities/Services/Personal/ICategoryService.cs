using FamilyBudgetTracker.Entities.Contracts.Personal.Category;

namespace FamilyBudgetTracker.Entities.Services.Personal;

public interface ICategoryService
{
    Task CreateCategory(CreateCategoryRequest request, string userId);

    Task UpdateCategory(int id, UpdateCategoryRequest request, string userId);

    Task DeleteCategory(int id, string userId);


    Task<CategoryResponse> GetCategory(int id);

    Task<List<CategoryResponse>> GetCategoriesForUser(string userId);
}