using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Transaction;

namespace FamilyBudgetTracker.BE.Commons.Services.Personal;

public interface IPersonalTransactionService
{
    Task CreateTransaction(CreatePersonalTransactionRequest request, string userId);
    
    Task UpdateTransaction(int id, UpdatePersonalTransactionRequest request, string userId);

    Task DeleteTransaction(int id, string userId);
    
    Task<PersonalTransactionResponse> GetTransactionById (int id, string userId);

    Task<List<PersonalTransactionResponse>> GetTransactionForPeriod(DateOnly startDate, DateOnly endDate, string userId);


}