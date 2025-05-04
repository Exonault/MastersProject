using FamilyBudgetTracker.Backend.Domain.Constants.Personal;
using FamilyBudgetTracker.Backend.Domain.Messages.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyCategory;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Familial.FamilyCategory;

public class CreateFamilyCategoryRequestValidator : AbstractValidator<CreateFamilyCategoryRequest>
{
    public CreateFamilyCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(FamilyCategoryValidationMessages.NameRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(FamilyCategoryValidationMessages.TypeRequired)
            .Must(x => CategoryConstants.Types.Contains(x))
            .WithMessage(FamilyCategoryValidationMessages.TypeMustBe);
    }
}