using FamilyBudgetTracker.Backend.Domain.Messages.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.Family;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Familial.Family;

public class CreateFamilyRequestValidator : AbstractValidator<CreateFamilyRequest>
{
    public CreateFamilyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(FamilyValidationMessages.NameRequired);

     
    }
}