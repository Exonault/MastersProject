using FamilyBudgetTracker.BE.Commons.Contracts.User;
using FamilyBudgetTracker.BE.Commons.Messages;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.User;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.AccessTokenRequired);

        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(UserMessages.ValidationMessages.RefreshTokenRequired);
    }
}