using FamilyBudgetTracker.BE.Commons.Contracts.Familial.Family;
using FamilyBudgetTracker.BE.Commons.Messages.Familial;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Familial.Family;

public class CreateFamilyRequestValidator : AbstractValidator<CreateFamilyRequest>
{
    public CreateFamilyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(FamilyMessages.NameRequired);

     
    }
}