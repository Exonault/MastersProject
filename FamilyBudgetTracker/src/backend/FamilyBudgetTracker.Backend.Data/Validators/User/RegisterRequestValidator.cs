using FamilyBudgetTracker.Backend.Domain.Messages.User;
using FamilyBudgetTracker.Shared.Contracts.User;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.User;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(UserValidationMessages.UserNameRequired);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserValidationMessages.EmailRequired)
            .EmailAddress()
            .WithMessage(UserValidationMessages.ProvidedEmailIsNotEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired);

        RuleFor(x => x.Admin)
            .NotNull()
            .WithMessage(UserValidationMessages.AdminRequired);

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage(UserValidationMessages.PasswordRequired)
            .MinimumLength(3).WithMessage(UserValidationMessages.PasswordMinLenght)
            // .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
            // .Matches(Regex.UpperCaseRegex)
            // .WithMessage(UserValidationMessages.PasswordUpperCaseRequirement)
            // .Matches(Regex.LowerCaseRegex)
            // .WithMessage(UserValidationMessages.PasswordLowerCaseRequirement)
            // .Matches(Regex.NumberRegex)
            // .WithMessage(UserValidationMessages.PasswordNumberRequirement)
            // .Matches(Regex.SpecialCharacterRegex)
            // .WithMessage(UserValidationMessages.PasswordSpecialCharacterRequirement)
            ;
    }
}