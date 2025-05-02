using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;

namespace FamilyBudgetTracker.BE.Commons.Services.Familial;

public interface IFamilyService
{
    Task<string> CreateFamily(CreateFamilyRequest request, string userId);

    Task AddFamilyMembersToFamily(AddFamilyMembersRequest request, string userId, string familyId);

    Task DeleteFamily(string id, string userId);

    Task<FamilyResponse> GetFamilyById(string id, string userId);

    Task<List<FamilyResponse>> GetAllFamilies();
}