using FamilyBudgetTracker.Backend.Messages.Familial;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Familial.Family;

public class CreateFamilyRequestValidator : AbstractValidator<CreateFamilyRequest>
{
    public CreateFamilyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(FamilyMessages.NameRequired);

        RuleFor(x => x.InviteList)
            .NotNull()
            .WithMessage(FamilyMessages.InvitesRequired);

        RuleForEach(x => x.InviteList)
            .EmailAddress()
            .WithMessage(FamilyMessages.ItemIsNotEmail);
    }
}