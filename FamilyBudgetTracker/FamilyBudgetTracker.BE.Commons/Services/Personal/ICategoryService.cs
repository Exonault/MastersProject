using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;

namespace FamilyBudgetTracker.BE.Commons.Services.Personal;

public interface ICategoryService
{
    Task CreateCategory(CreateCategoryRequest request, string userId);

    Task UpdateCategory(int id, UpdateCategoryRequest request, string userId);

    Task DeleteCategory(int id, string userId);


    Task<CategoryResponse> GetCategory(int id, string userId);

    Task<List<CategoryResponse>> GetCategoriesForUser(string userId);
}