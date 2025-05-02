using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Transaction;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Validators.Personal.PersonalTransaction;

public class UpdatePersonalTransactionRequestValidator : AbstractValidator<UpdatePersonalTransactionRequest>
{
    public UpdatePersonalTransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(PersonalTransactionMessages.AmountRequired)
            .Must(x => x > 0)
            .WithMessage(PersonalTransactionMessages.AmountMustBeMoreThanZero)
            .PrecisionScale(int.MaxValue, 2, true)
            .WithMessage(PersonalTransactionMessages.AmountValueMessage);

        // RuleFor(x => x.Description)
        //     .NotEmpty()
        //     .WithMessage(PersonalTransactionMessages.DescriptionRequired);

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(PersonalTransactionMessages.DateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(PersonalTransactionMessages.DateValueMessage);
    }
}