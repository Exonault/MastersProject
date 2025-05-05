using FamilyBudgetTracker.Backend.Data.Mappers;
using FamilyBudgetTracker.Backend.Data.Mappers.Familial;
using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Constants.User;
using FamilyBudgetTracker.Backend.Domain.Email;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FamilyBudgetTracker.Shared.Contracts.Familial.Invite;
using FamilyBudgetTracker.Shared.Contracts.User;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Familial;

public class FamilyService : IFamilyService
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISendEmailService _sendEmailService;
    private readonly IFamilyInvitationTokenRepository _familyInvitationTokenRepository;


    public FamilyService(IFamilyRepository familyRepository, IUserRepository userRepository,
        IFamilyInvitationTokenRepository familyInvitationTokenRepository, ISendEmailService sendEmailService)
    {
        _familyRepository = familyRepository;
        _userRepository = userRepository;
        _familyInvitationTokenRepository = familyInvitationTokenRepository;
        _sendEmailService = sendEmailService;
    }

    public async Task<string> CreateFamily(FamilyRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family family = new Family
        {
            Name = request.Name,
        };

        await _familyRepository.CreateFamily(family);

        user.Family = family;

        await _userRepository.UpdateUser(user);

        await _userRepository.AddToRole(user, UserConstants.RoleTypes.FamilyAdminRoleType);

        // string accessToken = await _generateTokenService.GenerateAccessToken(user);

        return "";
    }

    public async Task AddFamilyMembersToFamily(InviteFamilyMembersRequest request, string userId, string familyId)
    {
        //TODO send emails to join with code. 
        // Similar to https://www.youtube.com/watch?v=KtCjH-1iCIk

        List<string> inviteList = request.EmailList;

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        foreach (string email in inviteList)
        {
            User? user = await _userRepository.GetByEmail(email);

            bool userInApplicationFlag = user is null;

            var dateUtc = DateTime.UtcNow;
            var familyInvitationToken = new FamilyInvitationToken
            {
                Id = Guid.NewGuid(),
                Email = email,
                FamilyId = familyId,
                UserInApplication = userInApplicationFlag,
                CreatedOnUtc = dateUtc,
                ExpiresOnUtc = dateUtc.AddDays(1)
            };

            await _familyInvitationTokenRepository.CreateInvitationToken(familyInvitationToken);

            await _sendEmailService.SendFamilyInvitationEmail(familyInvitationToken);
        }
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

    public async Task<FamilyResponse> GetFamilyById(string id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(id);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        var familyMemberResponse = await GetFamilyMembersResponse(family.FamilyMembers);

        FamilyResponse familyResponse = family.ToFamilyResponse(familyMemberResponse);

        return familyResponse;
    }

    public async Task<List<FamilyResponse>> GetAllFamilies()
    {
        List<Family> allFamilies = await _familyRepository.GetAllFamilies();

        List<FamilyResponse> familyResponses = [];

        foreach (Family family in allFamilies)
        {
            List<UserResponse> familyMembersResponse = await GetFamilyMembersResponse(family.FamilyMembers);

            FamilyResponse familyResponse = family.ToFamilyResponse(familyMembersResponse);
            
            familyResponses.Add(familyResponse);
        }

        return familyResponses;
    }


    private async Task<List<UserResponse>> GetFamilyMembersResponse(List<User> familyMembers)
    {
        List<UserResponse> familyMemberResponse = [];

        foreach (User familyMember in familyMembers)
        {
            string mainFamilyRole = await _userRepository.GetMainFamilyRole(familyMember);

            UserResponse userResponse = familyMember.ToUserResponse(mainFamilyRole);

            familyMemberResponse.Add(userResponse);
        }

        return familyMemberResponse;
    }
}