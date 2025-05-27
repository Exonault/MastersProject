using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Transaction;

namespace FamilyBudgetTracker.Backend.Data.Mappers.Personal;

public static class PersonalTransactionMapper
{
    public static PersonalTransaction ToPersonalTransaction(this PersonalTransactionRequest request)
    {
        return new PersonalTransaction
        {
            Amount = request.Amount,
            Description = request.Description,
            TransactionDate = request.TransactionDate,
        };
    }


    public static PersonalTransaction ToPersonalTransaction(this PersonalTransactionRequest request,
        PersonalTransaction origin)
    {
        origin.Amount = request.Amount;
        origin.Description = request.Description;
        origin.TransactionDate = request.TransactionDate;

        return origin;
    }


    public static PersonalTransactionResponse ToPersonalTransactionResponse(this PersonalTransaction transaction)
    {
        return new PersonalTransactionResponse
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            Category = transaction.PersonalCategory.ToCategoryResponse(),
            // UserId = transaction.FamilyMember.Id
        };
    }

    public static PersonalTransaction ToPersonalTransaction(this RecurringTransaction recurringTransaction)
    {
        return new PersonalTransaction
        {
            Amount = recurringTransaction.Amount,
            Description = recurringTransaction.Description,
            TransactionDate = recurringTransaction.NextExecutionDate,
            PersonalCategory = recurringTransaction.PersonalCategory,
            User = recurringTransaction.User
        };
    }
}