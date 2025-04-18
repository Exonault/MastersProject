using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Constants.Personal;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.Entities.Contracts.Personal.Category;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validation.Personal;

public class CreateCategoryRequestValidator: AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(CategoryMessages.NameRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(CategoryMessages.TypeRequired)
            .Must(x => CategoryConstants.Types.Contains(x))
            .WithMessage(CategoryMessages.TypeMustBe);
    }
}