using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;
using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Mappers;
using FamilyBudgetTracker.BE.Commons.Mappers.Familial;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Familial;
using FamilyBudgetTracker.BE.Commons.Services;
using FamilyBudgetTracker.BE.Commons.Services.Auth;
using FamilyBudgetTracker.BE.Commons.Services.Familial;
using FamilyBudgetTracker.BE.Commons.Validation;

namespace FamilyBudgetTracker.Backend.Services.Familial;

public class FamilyService : IFamilyService
{
    private readonly IFamilyRepository _familyRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISendEmailService _sendEmailService;
    private readonly IFamilyInvitationTokenRepository _familyInvitationTokenRepository;
    private readonly IGenerateTokenService _generateTokenService;


    public FamilyService(IFamilyRepository familyRepository, IUserRepository userRepository, ISendEmailService sendEmailService,
        IFamilyInvitationTokenRepository familyInvitationTokenRepository, IGenerateTokenService generateTokenService)
    {
        _familyRepository = familyRepository;
        _userRepository = userRepository;
        _sendEmailService = sendEmailService;
        _familyInvitationTokenRepository = familyInvitationTokenRepository;
        _generateTokenService = generateTokenService;
    }

    public async Task<string> CreateFamily(CreateFamilyRequest request, string userId)
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

        await _userRepository.AddToRole(user, ApplicationConstants.RoleTypes.FamilyAdminRoleType);

        string accessToken = await _generateTokenService.GenerateAccessToken(user);

        return accessToken;
    }

    public async Task AddFamilyMembersToFamily(AddFamilyMembersRequest request, string userId, string familyId)
    {
        //TODO send emails to join with code. 
        // Similar to https://www.youtube.com/watch?v=KtCjH-1iCIk

        List<string> inviteList = request.InviteList;

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

        FamilyResponse familyResponse = family.ToFamilyResponse();

        familyResponse.FamilyMembers = familyMemberResponse;

        return familyResponse;
    }

    public async Task<List<FamilyResponse>> GetAllFamilies()
    {
        List<Family> allFamilies = await _familyRepository.GetAllFamilies();

        List<FamilyResponse> familyResponses = [];

        foreach (Family family in allFamilies)
        {
            FamilyResponse familyResponse = family.ToFamilyResponse();

            familyResponse.FamilyMembers = await GetFamilyMembersResponse(family.FamilyMembers);

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