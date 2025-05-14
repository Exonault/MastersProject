using FamilyBudgetTracker.Shared.Contracts.Personal.Transaction;

namespace FamilyBudgetTracker.Backend.Domain.Services.Personal;

public interface IPersonalTransactionService
{
    Task CreateTransaction(PersonalTransactionRequest request, string userId);
    
    Task UpdateTransaction(int id, PersonalTransactionRequest request, string userId);

    Task DeleteTransaction(int id, string userId);
    
    Task<PersonalTransactionResponse> GetTransactionById (int id, string userId);

    Task<List<PersonalTransactionResponse>> GetTransactionForPeriod(DateOnly startDate, DateOnly endDate, string userId);

    Task<PersonalTransactionPeriodSummaryResponse> GetTransactionsForPeriodSummary(DateOnly startDate, DateOnly endDate, string userId);
}