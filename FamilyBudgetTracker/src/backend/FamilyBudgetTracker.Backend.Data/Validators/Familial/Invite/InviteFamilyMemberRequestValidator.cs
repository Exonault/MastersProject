using FamilyBudgetTracker.Backend.Domain.Messages.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Invite;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Familial.Invite;

public class InviteFamilyMemberRequestValidator : AbstractValidator<InviteFamilyMembersRequest>
{
    public InviteFamilyMemberRequestValidator()
    {
        RuleFor(x => x.EmailList)
            .NotNull()
            .WithMessage(FamilyValidationMessages.InvitesRequired);

        RuleForEach(x => x.EmailList)
            .EmailAddress()
            .WithMessage(FamilyValidationMessages.ItemIsNotEmail);
    }
}