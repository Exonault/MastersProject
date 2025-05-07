using FamilyBudgetTracker.Backend.Data.Mappers;
using FamilyBudgetTracker.Backend.Data.Mappers.Familial;
using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Constants.User;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Familial;

public class FamilyService : IFamilyService
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUserRepository _userRepository;

    public FamilyService(IFamilyRepository familyRepository, IUserRepository userRepository)
    {
        _familyRepository = familyRepository;
        _userRepository = userRepository;
    }

    public async Task CreateFamily(FamilyRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        user = user.ValidateUserHasFamily();

        Family family = new Family
        {
            Name = request.Name,
        };

        await _familyRepository.CreateFamily(family);

        user.Family = family;

        await _userRepository.UpdateUser(user);

        await _userRepository.AddToRole(user, UserConstants.RoleTypes.FamilyAdminRoleType);
        await _userRepository.AddToRole(user, UserConstants.RoleTypes.FamilyMemberRoleType);
    }


    public async Task DeleteFamily(string id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(id);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        await _familyRepository.DeleteFamily(family);
    }

    public async Task<FamilyDetailedResponse> GetFamilyById(string id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(id);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        var familyMemberResponse = await GetFamilyMembersResponse(family.FamilyMembers);

        FamilyDetailedResponse familyDetailedResponse = family.ToFamilyDetailedResponse(familyMemberResponse);

        return familyDetailedResponse;
    }

    public async Task<List<FamilyDetailedResponse>> GetAllFamilies()
    {
        List<Family> allFamilies = await _familyRepository.GetAllFamilies();

        List<FamilyDetailedResponse> familyResponses = [];

        foreach (Family family in allFamilies)
        {
            List<FamilyMemberResponse> familyMembersResponse = await GetFamilyMembersResponse(family.FamilyMembers);

            FamilyDetailedResponse familyDetailedResponse = family.ToFamilyDetailedResponse(familyMembersResponse);
            
            familyResponses.Add(familyDetailedResponse);
        }

        return familyResponses;
    }


    private async Task<List<FamilyMemberResponse>> GetFamilyMembersResponse(List<User> familyMembers)
    {
        List<FamilyMemberResponse> familyMembersResponse = [];

        foreach (User familyMember in familyMembers)
        {
            string mainFamilyRole = await _userRepository.GetMainFamilyRole(familyMember);

            FamilyMemberResponse familyMemberResponse = familyMember.ToFamilyMemberResponse(mainFamilyRole);

            familyMembersResponse.Add(familyMemberResponse);
        }

        return familyMembersResponse;
    }
}