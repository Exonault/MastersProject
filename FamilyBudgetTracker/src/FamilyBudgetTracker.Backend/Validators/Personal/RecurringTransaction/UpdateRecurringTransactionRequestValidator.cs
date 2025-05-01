using FamilyBudgetTracker.Backend.Constants.Personal;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.RecurringTransaction;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Personal.RecurringTransaction;

public class UpdateRecurringTransactionRequestValidator : AbstractValidator<UpdateRecurringTransactionRequest>
{
    public UpdateRecurringTransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(RecurringTransactionMessages.AmountRequired)
            .Must(x => x > 0)
            .WithMessage(RecurringTransactionMessages.AmountMustBeMoreThanZero)
            .PrecisionScale(int.MaxValue, 2, true)
            .WithMessage(RecurringTransactionMessages.AmountValueMessage);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(RecurringTransactionMessages.DescriptionRequired);

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage(RecurringTransactionMessages.TypeRequired)
            .Must(x => RecurringTransactionConstants.Types.Contains(x))
            .WithMessage(RecurringTransactionMessages.TypeMustBe);

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage(RecurringTransactionMessages.StartDateRequired)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(RecurringTransactionMessages.StartDateValueMessage);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage(RecurringTransactionMessages.EndDateRequired)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => new { x.StartDate, x.EndDate })
            .Custom((dates, context) =>
            {
                if (dates.EndDate <= dates.StartDate)
                {
                    context.AddFailure("EndDate", RecurringTransactionMessages.EndDateValueMessage);
                }
            });
    }
}