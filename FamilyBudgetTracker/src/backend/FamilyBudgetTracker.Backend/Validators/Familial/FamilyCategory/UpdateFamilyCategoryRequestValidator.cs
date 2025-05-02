using FamilyBudgetTracker.Backend.Constants.Personal;
using FamilyBudgetTracker.Backend.Messages.Familial;
using FamilyBudgetTracker.BE.Commons.Contracts.Familial.FamilyCategory;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Familial.FamilyCategory;

public class UpdateFamilyCategoryRequestValidator :  AbstractValidator<UpdateFamilyCategoryRequest>
{
    public UpdateFamilyCategoryRequestValidator()
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