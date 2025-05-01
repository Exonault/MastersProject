using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.User;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.EmailRequired)
            .EmailAddress()
            .WithMessage(UserMessages.ValidationMessages.ProvidedEmailIsNotEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.PasswordRequired);
    }
}