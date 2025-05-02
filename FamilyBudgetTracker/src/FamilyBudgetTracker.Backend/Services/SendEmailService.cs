using FamilyBudgetTracker.BE.Commons.Entities.Familial;
using FamilyBudgetTracker.BE.Commons.Services;
using FluentEmail.Core;

namespace FamilyBudgetTracker.Backend.Services;

public class SendEmailService : ISendEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public SendEmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task SendTestEmail()
    {
        await _fluentEmail.To("kristiankrachmarov@gmail.com")
            .Subject("test subject")
            .Body("test body", isHtml: false)
            .SendAsync();
    }

    public async Task SendFamilyVerificationEmail(FamilyVerificationToken token)
    {
        await _fluentEmail.To(token.Email)
            .Subject("Family invite")
            .Body("You have been added to a family", isHtml: false)
            .SendAsync();
        
        throw new NotImplementedException();
    }
}