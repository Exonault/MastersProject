using FamilyBudgetTracker.BE.Commons.Entities.Familial;

namespace FamilyBudgetTracker.BE.Commons.Services;

public interface ISendEmailService
{
    Task SendTestEmail();

    Task SendFamilyInvitationEmail(FamilyInvitationToken token);
}