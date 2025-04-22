using FamilyBudgetTracker.BE.Commons.Entities.Personal;

namespace FamilyBudgetTracker.BE.Commons.Repositories.Personal;

public interface IRecurringTransactionRepository
{
    Task CreateRecurringTransaction(RecurringTransaction transaction);
    
    Task UpdateRecurringTransaction(RecurringTransaction transaction);
    
    Task DeleteRecurringTransaction(RecurringTransaction transaction);

    Task<RecurringTransaction?> GetRecurringTransactionById(int id);

    Task<List<RecurringTransaction>> GetRecurringTransactionsByUserid(string userId);
}