using FamilyBudgetTracker.BE.Commons.Services;
using FluentEmail.Core;

namespace FamilyBudgetTracker.Backend.Services;

public class EmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task SendTestEmail()
    {
        await _fluentEmail.To("kristiankrachmarov@gmail.com")
            .Subject("test subject")
            .Body("test body")
            .SendAsync();
    }
}