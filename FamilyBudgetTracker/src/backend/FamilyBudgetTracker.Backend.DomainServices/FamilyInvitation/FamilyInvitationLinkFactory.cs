using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FamilyBudgetTracker.Backend.DomainServices.FamilyInvitation.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace FamilyBudgetTracker.Backend.DomainServices.FamilyInvitation;

public class FamilyInvitationLinkFactory : IFamilyInvitationLinkFactory
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly LinkGenerator _linkGenerator;

    private readonly string? _frontendUrl;

    public FamilyInvitationLinkFactory(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator, IConfiguration configuration)
    {
        _contextAccessor = contextAccessor;
        _linkGenerator = linkGenerator;
        _frontendUrl = configuration["FrontEnd:AddFamilyMemberPage"];
    }


    public string Create(FamilyInvitations token)
    {
        if (_frontendUrl is null)
        {
            throw new Exception("Could not optain frontend url");
        }

        string invitationLink = $"{_frontendUrl}/{token.Id}";

        return invitationLink;
    }

    private string CreateOld(FamilyInvitations token)
    {
        string? invitationLink = _linkGenerator.GetUriByName(_contextAccessor.HttpContext!,
            FamilyInvitationConstants.JoinFamily,
            new { token = token.Id });

        return invitationLink ?? throw new Exception("Could not create family invitation link");
    }
}