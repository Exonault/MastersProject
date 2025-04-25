using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Transaction;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;

namespace FamilyBudgetTracker.Backend.Mappers.Personal;

public static class PersonalTransactionMapper
{
    public static PersonalTransaction ToPersonalTransaction(this CreatePersonalTransactionRequest request)
    {
        return new PersonalTransaction
        {
            Amount = request.Amount,
            Description = request.Description,
            TransactionDate = request.TransactionDate,
        };
    }


    public static PersonalTransaction ToPersonalTransaction(this UpdatePersonalTransactionRequest request)
    {
        return new PersonalTransaction
        {
            Amount = request.Amount,
            Description = request.Description,
            TransactionDate = request.TransactionDate,
        };
    }


    public static PersonalTransactionResponse ToPersonalTransactionResponse(this PersonalTransaction transaction)
    {
        return new PersonalTransactionResponse
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            Category = transaction.Category.ToCategoryResponse(),
            // UserId = transaction.User.Id
        };
    }

    public static PersonalTransaction ToPersonalTransaction(this RecurringTransaction recurringTransaction)
    {
        return new PersonalTransaction
        {
            Amount = recurringTransaction.Amount,
            Description = recurringTransaction.Description,
            TransactionDate = recurringTransaction.NextExecutionDate,
            Category = recurringTransaction.Category,
            User = recurringTransaction.User
        };
    }
}