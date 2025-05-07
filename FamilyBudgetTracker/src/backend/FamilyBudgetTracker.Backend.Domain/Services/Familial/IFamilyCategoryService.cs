using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyCategory;

namespace FamilyBudgetTracker.Backend.Domain.Services.Familial;

public interface IFamilyCategoryService
{
    Task CreateFamilyCategory(FamilyCategoryRequest request, string userId, string familyId);

    Task UpdateFamilyCategory(int id, FamilyCategoryRequest request, string userId, string familyId);

    Task DeleteFamilyCategory(int id, string userId, string familyId);

    Task<FamilyCategorySingleResponse> GetFamilyCategoryById(int id, string userId,  string familyId);

    Task<List<FamilyCategoryResponse>> GetFamilyCategoriesByFamilyId(string familyId, string userId);
}