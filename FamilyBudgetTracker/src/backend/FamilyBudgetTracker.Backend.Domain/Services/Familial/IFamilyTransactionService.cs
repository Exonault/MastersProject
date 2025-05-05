using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyTransaction;

namespace FamilyBudgetTracker.Backend.Domain.Services.Familial;

public interface IFamilyTransactionService
{
    Task CreateFamilyTransaction(FamilyTransactionRequest request, string userId, string familyId);

    Task UpdateFamilyTransaction(int id, FamilyTransactionRequest request, string userId, string familyId);

    Task DeleteFamilyTransaction(int id, string userId, string familyId);

    Task<FamilyTransactionResponse> GetFamilyTransactionById(int id, string userId, string familyId);

    Task<List<FamilyTransactionResponse>> GetFamilyTransactionsForPeriod(DateOnly startDate, DateOnly endDate, string userId,
        string familyId);

    Task<FamilyTransactionsForPeriodSummaryResponse> GetFamilyTransactionsForPeriodSummary(DateOnly startDate, DateOnly endDate,
        string userId, string familyId);
}