using FamilyBudgetTracker.Backend.Domain.Entities.Personal;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Personal;

public interface IRecurringTransactionRepository
{
    Task CreateRecurringTransaction(RecurringTransaction transaction);
    
    Task UpdateRecurringTransaction(RecurringTransaction transaction);
    
    Task DeleteRecurringTransaction(RecurringTransaction transaction);

    Task<RecurringTransaction?> GetRecurringTransactionById(int id);

    Task<List<RecurringTransaction>> GetRecurringTransactionsByUserid(string userId);
}