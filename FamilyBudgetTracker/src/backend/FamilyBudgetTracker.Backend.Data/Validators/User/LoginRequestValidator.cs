using FamilyBudgetTracker.Backend.Domain.Messages.User;
using FamilyBudgetTracker.Shared.Contracts.User;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.User;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserValidationMessages.EmailRequired)
            .EmailAddress()
            .WithMessage(UserValidationMessages.ProvidedEmailIsNotEmail);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired);
    }
}