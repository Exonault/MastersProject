using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.BE.Entities.Contracts.User;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validation.User;

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