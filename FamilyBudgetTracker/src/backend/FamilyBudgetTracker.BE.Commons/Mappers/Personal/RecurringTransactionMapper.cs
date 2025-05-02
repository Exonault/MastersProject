using FamilyBudgetTracker.BE.Commons.Contracts.Personal.RecurringTransaction;
using FamilyBudgetTracker.BE.Commons.Entities.Common;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;

namespace FamilyBudgetTracker.BE.Commons.Mappers.Personal;

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
            EndDate = request.EndDate,
        };
    }


    public static RecurringTransaction ToRecurringTransaction(this UpdateRecurringTransactionRequest request,
        RecurringTransaction origin)
    {
        origin.Amount = request.Amount;
        origin.Description = request.Description;
        origin.Type = Enum.Parse<RecurringType>(request.Type);
        origin.StartDate = request.StartDate;
        origin.EndDate = request.EndDate;

        return origin;
    }


    public static RecurringTransactionResponse ToRecurringTransactionResponse(this RecurringTransaction transaction)
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