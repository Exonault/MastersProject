using FamilyBudgetTracker.BE.Commons.Entities.Familial;

namespace FamilyBudgetTracker.BE.Commons.Repositories.Familial;

public interface IFamilyCategoryRepository
{
    Task CreateFamilyCategory(FamilyCategory category);
    
    Task UpdateFamilyCategory(FamilyCategory category);

    Task DeleteFamilyCategory(FamilyCategory category);

    Task<FamilyCategory?> GetCategoryById(int id);

    Task<List<FamilyCategory>> GetCategoriesByFamilyId(string familyId);
}