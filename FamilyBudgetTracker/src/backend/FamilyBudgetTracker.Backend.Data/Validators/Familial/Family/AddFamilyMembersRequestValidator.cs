using FamilyBudgetTracker.Backend.Domain.Messages.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Familial.Family;

public class AddFamilyMembersRequestValidator : AbstractValidator<InviteFamilyMembersRequest>
{
    public AddFamilyMembersRequestValidator()
    {
        RuleFor(x => x.EmailList)
            .NotNull()
            .WithMessage(FamilyValidationMessages.InvitesRequired);

        RuleForEach(x => x.EmailList)
            .EmailAddress()
            .WithMessage(FamilyValidationMessages.ItemIsNotEmail);
    }
}