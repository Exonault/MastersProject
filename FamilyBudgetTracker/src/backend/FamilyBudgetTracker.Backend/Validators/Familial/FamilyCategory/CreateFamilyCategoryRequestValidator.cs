using FamilyBudgetTracker.BE.Commons.Constants.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;
using FamilyBudgetTracker.BE.Commons.Messages.Familial;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Familial.FamilyCategory;

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