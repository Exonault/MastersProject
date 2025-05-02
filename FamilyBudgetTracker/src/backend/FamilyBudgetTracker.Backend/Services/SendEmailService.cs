using FamilyBudgetTracker.Backend.Factory;
using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Services;
using FluentEmail.Core;

namespace FamilyBudgetTracker.Backend.Services;

public class SendEmailService : ISendEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly FamilyInvitationLinkFactory _invitationLinkFactory;

    public SendEmailService(IFluentEmail fluentEmail, FamilyInvitationLinkFactory invitationLinkFactory)
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