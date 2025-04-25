using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.BE.Entities.Contracts.User;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validation.User;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.UserNameRequired);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.EmailRequired)
            .EmailAddress()
            .WithMessage(UserMessages.ValidationMessages.ProvidedEmailIsNotEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.PasswordRequired);

        RuleFor(x => x.Admin)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.AdminRequired);

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage(UserMessages.ValidationMessages.PasswordRequired)
            .MinimumLength(3).WithMessage(UserMessages.ValidationMessages.PasswordMinLenght);
        // .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
        // .Matches(ApplicationConstants.Regex.UpperCaseRegex)
        // .WithMessage(UserMessages.ValidationMessages.PasswordUpperCaseRequirement)
        // .Matches(ApplicationConstants.Regex.LowerCaseRegex)
        // .WithMessage(UserMessages.ValidationMessages.PasswordLowerCaseRequirement)
        // .Matches(ApplicationConstants.Regex.NumberRegex)
        // .WithMessage(UserMessages.ValidationMessages.PasswordNumberRequirement)
        // .Matches(ApplicationConstants.Regex.SpecialCharacterRegex)
        // .WithMessage(UserMessages.ValidationMessages.PasswordSpecialCharacterRequirement);
    }
}