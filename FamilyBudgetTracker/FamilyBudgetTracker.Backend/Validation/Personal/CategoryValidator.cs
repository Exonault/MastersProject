using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.Entities.Entities.Personal;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validation.Personal;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(CategoryMessages.NameRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(CategoryMessages.TypeRequired);
    }
}