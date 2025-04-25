using FamilyBudgetTracker.BE.Commons.Contracts.Personal.RecurringTransaction;
using FamilyBudgetTracker.BE.Commons.Services.Personal;

namespace FamilyBudgetTracker.Backend.Services.Personal;

public class RecurringTransactionService : IRecurringTransactionService
{
    
    //TODO for all: calculate the NextExecutionDate 
    public Task CreateRecurringTransaction(CreateRecurringTransactionRequest request, string userId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRecurringTransaction(int id, UpdateRecurringTransactionRequest request, string userId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRecurringTransaction(int id, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<RecurringTransactionResponse> GetRecurringTransactionById(int id, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<RecurringTransactionResponse>> GetRecurringTransactionsForUser(string userId)
    {
        throw new NotImplementedException();
    }
}