using FamilyBudgetTracker.Frontend.Contracts.Personal.Category;
using FamilyBudgetTracker.Frontend.Models.Category;

namespace FamilyBudgetTracker.Frontend.Interfaces;

public interface ICategoriesService
{
   Task<List<CategoryResponse>> GetCategories(string token, string refreshToken, string userId);

   Task<CategoryModel> GetCategoryModel(int id, string token, string refreshToken, string userId);

   Task<CategoryResponse> GetCategory(int id, string token, string refreshToken, string userId);
}