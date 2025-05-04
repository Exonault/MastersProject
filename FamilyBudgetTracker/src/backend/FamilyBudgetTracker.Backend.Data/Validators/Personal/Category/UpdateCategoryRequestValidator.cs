using FamilyBudgetTracker.Backend.Domain.Constants.Personal;
using FamilyBudgetTracker.Backend.Domain.Messages.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Category;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Personal.Category;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(CategoryValidationMessages.NameRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(CategoryValidationMessages.TypeRequired)
            .Must(x => CategoryConstants.Types.Contains(x))
            .WithMessage(CategoryValidationMessages.TypeMustBe);
    }
}