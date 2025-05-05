using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyTransaction;

namespace FamilyBudgetTracker.Backend.Data.Mappers.Familial;

public static class FamilyTransactionMapper
{
    public static FamilyTransaction ToFamilyTransaction(this FamilyTransactionRequest request)
    {
        return new FamilyTransaction
        {
            Amount = request.Amount,
            Description = request.Description,
            TransactionDate = request.TransactionDate,
        };
    }

    public static FamilyTransaction ToFamilyTransaction(this FamilyTransactionRequest request, FamilyTransaction origin)
    {
        origin.Amount = request.Amount;
        origin.Description = request.Description;
        origin.TransactionDate = request.TransactionDate;


        return origin;
    }


    public static FamilyTransactionResponse ToFamilyTransactionResponse(this FamilyTransaction transaction, string userRole)
    {
        return new FamilyTransactionResponse
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            Category = transaction.Category.ToFamilyCategoryResponse(),
            User = transaction.User.ToUserResponse(userRole)
        };
    }
}