using FamilyBudgetTracker.Entities.Entities.Personal;

namespace FamilyBudgetTracker.Entities.Repositories.Personal;

public interface IRecurringTransactionRepository
{
    Task CreateRecurringTransaction(RecurringTransaction transaction);
    
    Task UpdateRecurringTransaction(RecurringTransaction transaction);
    
    Task DeleteRecurringTransaction(RecurringTransaction transaction);

    Task<RecurringTransaction> GetRecurringTransactionById(int id);

    Task<RecurringTransaction> GetRecurringTransactionsByUserid(string userId);
}