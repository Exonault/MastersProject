using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;

namespace FamilyBudgetTracker.BE.Commons.Services.Familial;

public interface IFamilyCategoryService
{
    Task CreateFamilyCategory(CreateFamilyCategoryRequest request, string userId);

    Task UpdateFamilyCategory(int id, UpdateFamilyCategoryRequest request, string userId);

    Task DeleteFamilyCategory(int id, string userId);

    Task<FamilyCategoryResponse> GetFamilyCategoryById(int id, string userId);

    Task<List<FamilyCategoryResponse>> GetFamilyCategoriesByFamilyId(int familyId, string userId);
    
}