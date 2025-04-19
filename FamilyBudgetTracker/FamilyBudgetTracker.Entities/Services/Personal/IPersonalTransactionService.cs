using FamilyBudgetTracker.Entities.Contracts.Personal.Transaction;

namespace FamilyBudgetTracker.Entities.Services.Personal;

public interface IPersonalTransactionService
{
    Task CreateTransaction(CreatePersonalTransactionRequest request, string userId);
    
    Task UpdateTransaction(int id, UpdatePersonalTransactionRequest request, string userId);

    Task DeleteTransaction(int id, string userId);
    
    Task<PersonalTransactionResponse> GetTransactionById (int id, string userId);

    Task<List<PersonalTransactionResponse>> GetTransactionForPeriod(PersonalTransactionsForPeriodRequest request, string userId);


}