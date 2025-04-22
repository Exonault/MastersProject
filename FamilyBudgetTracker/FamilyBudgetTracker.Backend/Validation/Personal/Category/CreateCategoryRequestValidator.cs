using FamilyBudgetTracker.Backend.Constants.Personal;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validation.Personal.Category;

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