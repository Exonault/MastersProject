using FamilyBudgetTracker.Backend.Domain.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Domain.Email;

public interface ISendEmailService
{
    Task SendTestEmail();

    Task SendFamilyInvitationEmail(FamilyInvitationToken token);
    // Task SendFamilyInvitationEmail();
}