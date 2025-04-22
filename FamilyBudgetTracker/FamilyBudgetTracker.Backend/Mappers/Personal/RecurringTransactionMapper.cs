using FamilyBudgetTracker.BE.Commons.Contracts.Personal.RecurringTransaction;
using FamilyBudgetTracker.BE.Commons.Entities.Common;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;

namespace FamilyBudgetTracker.Backend.Mappers.Personal;

public static class RecurringTransactionMapper
{
    public static RecurringTransaction ToRecurringTransaction(this CreateRecurringTransactionRequest request)
    {
        return new RecurringTransaction
        {
            Amount = request.Amount,
            Description = request.Description,
            Type = Enum.Parse<RecurringType>(request.Type),
            StartDate = request.StartDate,
            NextExecutionDate = request.NextExecutionDate,
            EndDate = request.EndDate,
        };
    }


    public static RecurringTransaction ToRecurringTransaction(this UpdateRecurringTransactionRequest request)
    {
        return new RecurringTransaction()
        {
            Amount = request.Amount,
            Description = request.Description,
            Type = Enum.Parse<RecurringType>(request.Type),
            StartDate = request.StartDate,
            NextExecutionDate = request.NextExecutionDate,
            EndDate = request.EndDate,
        };
    }


    public static RecurringTransactionResponse ToRecurringTransaction(this RecurringTransaction transaction)
    {
        return new RecurringTransactionResponse
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Type = transaction.Type.ToString(),
            StartDate = transaction.StartDate,
            NextExecutionDate = transaction.NextExecutionDate,
            EndDate = transaction.EndDate,
            Category = transaction.Category.ToCategoryResponse()
        };
    }
}