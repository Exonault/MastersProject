using FamilyBudgetTracker.Backend.Domain.Constants.Personal;
using FamilyBudgetTracker.Backend.Domain.Messages.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.RecurringTransaction;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Personal.RecurringTransaction;

public class CreateRecurringTransactionRequestValidator : AbstractValidator<RecurringTransactionRequest>
{
    public CreateRecurringTransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(RecurringTransactionValidationMessages.AmountRequired)
            .Must(x => x > 0)
            .WithMessage(RecurringTransactionValidationMessages.AmountMustBeMoreThanZero)
            .PrecisionScale(int.MaxValue, 2, true)
            .WithMessage(RecurringTransactionValidationMessages.AmountValueMessage);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(RecurringTransactionValidationMessages.DescriptionRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(RecurringTransactionValidationMessages.TypeRequired)
            .Must(x => RecurringTransactionConstants.Types.Contains(x))
            .WithMessage(RecurringTransactionValidationMessages.TypeMustBe);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage(RecurringTransactionValidationMessages.StartDateRequired)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(RecurringTransactionValidationMessages.StartDateValueMessage);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage(RecurringTransactionValidationMessages.EndDateRequired)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => new { x.StartDate, x.EndDate })
            .Custom((dates, context) =>
            {
                if (dates.EndDate <= dates.StartDate)
                {
                    context.AddFailure("EndDate", RecurringTransactionValidationMessages.EndDateValueMessage);
                }
            });
    }
}