using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;
using FamilyBudgetTracker.BE.Commons.Messages.Familial;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Familial.Family;

public class AddFamilyMembersRequestValidator : AbstractValidator<AddFamilyMembersRequest>
{
    public AddFamilyMembersRequestValidator()
    {
        RuleFor(x => x.InviteList)
            .NotNull()
            .WithMessage(FamilyMessages.InvitesRequired);

        RuleForEach(x => x.InviteList)
            .EmailAddress()
            .WithMessage(FamilyMessages.ItemIsNotEmail);
    }
}