using FamilyBudgetTracker.Shared.Contracts.Familial.Family;

namespace FamilyBudgetTracker.Backend.Domain.Invite;

public interface IFamilyInvitationService
{
    Task InviteMembersToFamily(InviteFamilyMembersRequest request,  string familyId);

    Task AddUserToFamily(string tokenId);
}