using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data.Repositories.Personal;

public class RecurringTransactionRepository : IRecurringTransactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RecurringTransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateRecurringTransaction(RecurringTransaction transaction)
    {
        await _dbContext.RecurringTransactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRecurringTransaction(RecurringTransaction transaction)
    {
        _dbContext.Entry(transaction).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRecurringTransaction(RecurringTransaction transaction)
    {
        _dbContext.RecurringTransactions.Remove(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RecurringTransaction?> GetRecurringTransactionById(int id)
    {
        return await _dbContext.RecurringTransactions
            .Include(rt => rt.User)
            .Include(rt => rt.PersonalCategory)
            .FirstOrDefaultAsync(rt => rt.Id == id);
    }

    public async Task<List<RecurringTransaction>> GetRecurringTransactionsByUserid(string userId)
    {
        return await _dbContext.RecurringTransactions
            .Include(rt => rt.User)
            .Include(rt => rt.PersonalCategory)
            .Where(rt => rt.User.Id == userId)
            .ToListAsync();
    }

    public async Task<List<RecurringTransaction>> GetRecurringTransactionsByExecutionDate(DateOnly date)
    {
        return await _dbContext.RecurringTransactions
            .Include(rt => rt.User)
            .Include(rt => rt.PersonalCategory)
            .Where(rt => rt.NextExecutionDate == date)
            .ToListAsync();
    }
}