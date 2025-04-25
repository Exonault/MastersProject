using FamilyBudgetTracker.BE.Commons.Contracts.Personal.RecurringTransaction;

namespace FamilyBudgetTracker.BE.Commons.Services.Personal;

public interface IRecurringTransactionService
{
    Task CreateRecurringTransaction(CreateRecurringTransactionRequest request, string userId);
    
    Task UpdateRecurringTransaction(int id, UpdateRecurringTransactionRequest request, string userId);
    
    Task DeleteRecurringTransaction(int id, string userId);

    Task<RecurringTransactionResponse> GetRecurringTransactionById(int id, string userId);

    Task<List<RecurringTransactionResponse>> GetRecurringTransactionsForUser(string userId);

}