using FamilyBudgetTracker.Backend.Domain.Email;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FluentEmail.Core;

namespace FamilyBudgetTracker.Backend.Email.Services;

public class SendEmailService : ISendEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly IFamilyInvitationLinkFactory _invitationLinkFactory;

    public SendEmailService(IFluentEmail fluentEmail, IFamilyInvitationLinkFactory invitationLinkFactory)
    {
        _fluentEmail = fluentEmail;
        _invitationLinkFactory = invitationLinkFactory;
    }


    public async Task SendTestEmail()
    {
        await _fluentEmail.To("test@test.test")
            .Subject("test subject")
            .Body("test body", isHtml: false)
            .SendAsync();
    }

    public async Task SendFamilyInvitationEmail(FamilyInvitationToken token)
    {
        string invitationLink = _invitationLinkFactory.Create(token);

        await _fluentEmail.To(token.Email)
            .Subject("Family invite")
            .Body($"You have been invited to join family. <a href='{invitationLink}'>Click here</a>", isHtml: true)
            .SendAsync();
        
        throw new NotImplementedException();
    }
}