using FamilyBudgetTracker.Shared.Contracts.Personal.RecurringTransaction;

namespace FamilyBudgetTracker.Backend.Domain.Services.Personal;

public interface IRecurringTransactionService
{
    Task CreateRecurringTransaction(CreateRecurringTransactionRequest request, string userId);
    
    Task UpdateRecurringTransaction(int id, UpdateRecurringTransactionRequest request, string userId);
    
    Task DeleteRecurringTransaction(int id, string userId);

    Task<RecurringTransactionResponse> GetRecurringTransactionById(int id, string userId);

    Task<List<RecurringTransactionResponse>> GetRecurringTransactionsForUser(string userId);

}