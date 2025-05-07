using FamilyBudgetTracker.Backend.Domain.Email;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Invite;
using FluentEmail.Core;

namespace FamilyBudgetTracker.Backend.Email.Services;

public class SendEmailService : ISendEmailService
{
    private readonly IFluentEmailFactory _fluentEmailFactory;
    private readonly IFamilyInvitationLinkFactory _invitationLinkFactory;

    public SendEmailService(IFluentEmailFactory fluentEmailFactory, IFamilyInvitationLinkFactory invitationLinkFactory)
    {
        _fluentEmailFactory = fluentEmailFactory;
        _invitationLinkFactory = invitationLinkFactory;
    }


    public async Task SendTestEmail()
    {
        await _fluentEmailFactory
            .Create()
            .To("test@test.test")
            .Subject("test subject")
            .Body("test body", isHtml: false)
            .SendAsync();
    }

    public async Task SendFamilyInvitationEmail(FamilyInvitationToken token)
    {
        string invitationLink = _invitationLinkFactory.Create(token);

        await _fluentEmailFactory
            .Create()
            .To(token.Email)
            .Subject("Family invite")
            .Body($"You have been invited to join family. <a href='{invitationLink}'>Click here</a>", isHtml: true)
            .SendAsync();
    }
}