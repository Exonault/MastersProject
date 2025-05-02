using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;

namespace FamilyBudgetTracker.BE.Commons.Services.Familial;

public interface IFamilyService
{
    Task CreateFamily(CreateFamilyRequest request, string userId);

    Task AddFamilyMembersToFamily(AddFamilyMembersRequest request, string userId, int familyId);

    Task DeleteFamily(string id, string userId);

    Task<FamilyResponse> GetFamilyById(string id, string userId);

    Task<List<FamilyResponse>> GetAllFamilies();
}