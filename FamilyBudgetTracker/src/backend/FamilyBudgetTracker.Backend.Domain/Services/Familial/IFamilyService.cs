using FamilyBudgetTracker.Shared.Contracts.Familial.Family;

namespace FamilyBudgetTracker.Backend.Domain.Services.Familial;

public interface IFamilyService
{
    Task CreateFamily(FamilyRequest request, string userId);

    Task DeleteFamily(string id, string userId);

    Task<FamilyResponse> GetFamilyById(string id, string userId);

    Task<List<FamilyResponse>> GetAllFamilies();
}