using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Factory;

public class FamilyInvitationLinkFactory
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
            ApplicationConstants.FamilyJoining.JoinFamily,
            new { token = token.Id });

        return invitationLink ?? throw new Exception("Could not create family invitation link");
    }
}