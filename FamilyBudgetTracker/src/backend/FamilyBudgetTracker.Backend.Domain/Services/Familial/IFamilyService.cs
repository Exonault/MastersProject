using FamilyBudgetTracker.Shared.Contracts.Familial.Family;

namespace FamilyBudgetTracker.Backend.Domain.Services.Familial;

public interface IFamilyService
{
    Task<string> CreateFamily(CreateFamilyRequest request, string userId);

    Task AddFamilyMembersToFamily(InviteFamilyMembersRequest request, string userId, string familyId);

    Task DeleteFamily(string id, string userId);

    Task<FamilyResponse> GetFamilyById(string id, string userId);

    Task<List<FamilyResponse>> GetAllFamilies();
}