using FamilyBudgetTracker.Backend.Constants.Personal;
using FamilyBudgetTracker.Backend.Messages.Familial;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validation.Familial.FamilyCategory;

public class CreateFamilyCategoryRequestValidator : AbstractValidator<CreateFamilyCategoryRequest>
{
    public CreateFamilyCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(FamilyCategoryMessages.NameRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(FamilyCategoryMessages.TypeRequired)
            .Must(x => CategoryConstants.Types.Contains(x))
            .WithMessage(FamilyCategoryMessages.TypeMustBe);
    }
}