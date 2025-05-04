using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Invite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FamilyBudgetTracker.Backend.DomainServices.FamilyInvitation;

public class FamilyInvitationLinkFactory : IFamilyInvitationLinkFactory
{
    private IHttpContextAccessor _contextAccessor;
    private LinkGenerator _linkGenerator;

    public FamilyInvitationLinkFactory(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
    {
        _contextAccessor = contextAccessor;
        _linkGenerator = linkGenerator;
    }


    public string Create(FamilyInvitationToken token)
    {
        string? invitationLink = _linkGenerator.GetUriByName(_contextAccessor.HttpContext!,
            FamilyInvitationConstants.JoinFamily,
            new { token = token.Id });

        return invitationLink ?? throw new Exception("Could not create family invitation link");
    }
}