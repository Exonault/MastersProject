using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;

namespace FamilyBudgetTracker.BE.Commons.Services.Familial;

public interface IFamilyService
{
    Task CreateFamily(CreateFamilyRequest request, string userId);

    Task DeleteFamily(int id, string userId);

    Task<FamilyResponse> GetFamilyById(int id, string userId);

    Task<List<FamilyResponse>> GetAllFamilies();
}