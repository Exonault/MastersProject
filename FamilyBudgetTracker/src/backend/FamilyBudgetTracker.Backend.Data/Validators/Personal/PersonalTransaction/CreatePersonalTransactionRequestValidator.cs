using FamilyBudgetTracker.Backend.Domain.Messages.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Transaction;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Data.Validators.Personal.PersonalTransaction;

public class CreatePersonalTransactionRequestValidator : AbstractValidator<CreatePersonalTransactionRequest>
{
    public CreatePersonalTransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(PersonalTransactionValdationMessages.AmountRequired)
            .Must(x => x > 0)
            .WithMessage(PersonalTransactionValdationMessages.AmountMustBeMoreThanZero)
            .PrecisionScale(int.MaxValue, 2, true)
            .WithMessage(PersonalTransactionValdationMessages.AmountValueMessage);

        // RuleFor(x => x.Description)
        //     .NotEmpty()
        //     .WithMessage(PersonalTransactionMessages.DescriptionRequired);

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(PersonalTransactionValdationMessages.DateRequired)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage(PersonalTransactionValdationMessages.DateValueMessage);
    }
}