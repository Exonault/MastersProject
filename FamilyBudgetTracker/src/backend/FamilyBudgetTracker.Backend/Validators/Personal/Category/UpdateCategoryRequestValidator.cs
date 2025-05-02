using FamilyBudgetTracker.BE.Commons.Constants.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Category;
using FamilyBudgetTracker.BE.Commons.Messages.Personal;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Personal.Category;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
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