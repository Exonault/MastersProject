using BooksAPI.FE.Contracts.Familial.FamilyCategory;
using BooksAPI.FE.Model;

namespace BooksAPI.FE.Interfaces;

public interface IFamilyCategoryService
{
    Task<List<FamilyCategoryResponse>> GetFamilyCategories(string token, string refreshToken, string userId,
        string familyId);

    Task<FamilyCategoryModel> GetCategoryModel(int id, string token, string refreshToken, string userId,
        string familyId);

    Task<FamilyCategoryResponse> GetCategory(int id, string token, string refreshToken, string userId, string familyId);

    Task<bool> CreateCategory(FamilyCategoryModel model, string token, string refreshToken, string userId,
        string familyId);

    Task<bool> UpdateCategory(int id, FamilyCategoryModel model, string token, string refreshToken, string userId,
        string familyId);

    Task<bool> DeleteCategory(int id, string token, string refreshToken, string userId, string familyId);
}