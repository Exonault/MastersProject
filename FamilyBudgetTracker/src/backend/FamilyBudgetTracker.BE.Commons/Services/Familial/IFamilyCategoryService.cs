using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;

namespace FamilyBudgetTracker.BE.Commons.Services.Familial;

public interface IFamilyCategoryService
{
    Task CreateFamilyCategory(CreateFamilyCategoryRequest request, string userId, string familyId);

    Task UpdateFamilyCategory(int id, UpdateFamilyCategoryRequest request, string userId, string familyId);

    Task DeleteFamilyCategory(int id, string userId, string familyId);

    Task<FamilyCategoryResponse> GetFamilyCategoryById(int id, string userId,  string familyId);

    Task<List<FamilyCategoryResponse>> GetFamilyCategoriesByFamilyId(string familyId, string userId);
}