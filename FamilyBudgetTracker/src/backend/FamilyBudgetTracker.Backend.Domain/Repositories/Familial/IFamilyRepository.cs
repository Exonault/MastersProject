using FamilyBudgetTracker.Backend.Domain.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Familial;

public interface IFamilyRepository
{
    Task CreateFamily(Family family);

    Task DeleteFamily(Family family);

    Task<Family?> GetFamilyById(string id);

    Task<List<Family>> GetAllFamilies();
}