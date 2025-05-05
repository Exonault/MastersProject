using FamilyBudgetTracker.Backend.Domain.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Familial;

public interface IFamilyCategoryRepository
{
    Task CreateFamilyCategory(FamilyCategory category);
    
    Task UpdateFamilyCategory(FamilyCategory category);

    Task DeleteFamilyCategory(FamilyCategory category);

    Task<FamilyCategory?> GetFamilyCategoryById(int id);

    Task<List<FamilyCategory>> GetFamilyCategoriesByFamilyId(string familyId);
}