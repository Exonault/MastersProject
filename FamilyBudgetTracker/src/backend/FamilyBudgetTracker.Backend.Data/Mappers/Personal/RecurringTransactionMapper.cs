using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.RecurringTransaction;

namespace FamilyBudgetTracker.Backend.Data.Mappers.Personal;

public static class RecurringTransactionMapper
{
    public static RecurringTransaction ToRecurringTransaction(this RecurringTransactionRequest request)
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


    public static RecurringTransaction ToRecurringTransaction(this RecurringTransactionRequest request,
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
            Category = transaction.PersonalCategory.ToCategoryResponse()
        };
    }
}