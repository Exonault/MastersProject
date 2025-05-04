using FamilyBudgetTracker.Backend.Domain.Messages.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Familial.Family;

public class AddFamilyMembersRequestValidator : AbstractValidator<AddFamilyMembersRequest>
{
    public AddFamilyMembersRequestValidator()
    {
        RuleFor(x => x.InviteList)
            .NotNull()
            .WithMessage(FamilyValidationMessages.InvitesRequired);

        RuleForEach(x => x.InviteList)
            .EmailAddress()
            .WithMessage(FamilyValidationMessages.ItemIsNotEmail);
    }
}