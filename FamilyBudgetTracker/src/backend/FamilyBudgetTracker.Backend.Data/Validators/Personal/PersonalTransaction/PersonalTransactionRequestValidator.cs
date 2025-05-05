using FamilyBudgetTracker.Backend.Domain.Messages.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Transaction;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Personal.PersonalTransaction;

public class PersonalTransactionRequestValidator : AbstractValidator<PersonalTransactionRequest>
{
    public PersonalTransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(PersonalTransactionValidationMessages.AmountRequired)
            .Must(x => x > 0)
            .WithMessage(PersonalTransactionValidationMessages.AmountMustBeMoreThanZero)
            .PrecisionScale(int.MaxValue, 2, true)
            .WithMessage(PersonalTransactionValidationMessages.AmountValueMessage);

        // RuleFor(x => x.Description)
        //     .NotEmpty()
        //     .WithMessage(PersonalTransactionMessages.DescriptionRequired);

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(PersonalTransactionValidationMessages.DateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(PersonalTransactionValidationMessages.DateValueMessage);

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage(PersonalTransactionValidationMessages.CategoryIsRequired);
    }
}