using BooksAPI.FE.Contracts.Personal.Category;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Interfaces;

public interface ICategoriesService
{
   Task<List<CategoryResponse>> GetCategories(string token, string refreshToken, string userId);

   Task<CategoryModel> GetCategoryModel(int id, string token, string refreshToken, string userId);

   Task<CategoryResponse> GetCategory(int id, string token, string refreshToken, string userId);
   
   Task<bool> CreateCategory(CategoryModel model, string token, string refreshToken, string userId);
   Task<bool> UpdateCategory(int id, CategoryModel model, string token, string refreshToken, string userId);
}