using FamilyBudgetTracker.Backend.Domain.Messages.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyTransaction;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Familial.FamilyTransaction;

public class FamilyTransactionRequestValidator : AbstractValidator<FamilyTransactionRequest>
{
    public FamilyTransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(FamilyTransactionValidationMessages.AmountRequired)
            .Must(x => x > 0)
            .WithMessage(FamilyTransactionValidationMessages.AmountMustBeMoreThanZero)
            .PrecisionScale(int.MaxValue, 2, true)
            .WithMessage(FamilyTransactionValidationMessages.AmountValueMessage);

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(FamilyTransactionValidationMessages.DateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(FamilyTransactionValidationMessages.DateValueMessage);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(FamilyTransactionValidationMessages.DescriptionRequired);

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(FamilyTransactionValidationMessages.DateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(FamilyTransactionValidationMessages.DateValueMessage);

        RuleFor(x => x.FamilyCategoryId)
            .NotEmpty()
            .WithMessage(FamilyTransactionValidationMessages.CategoryIsRequired);
    }
}