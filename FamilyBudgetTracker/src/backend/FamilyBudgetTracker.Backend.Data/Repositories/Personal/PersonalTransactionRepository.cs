using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data.Repositories.Personal;

public class PersonalTransactionRepository : IPersonalTransactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PersonalTransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task CreateTransaction(PersonalTransaction transaction)
    {
        await _dbContext.PersonalTransactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTransaction(PersonalTransaction transaction)
    {
        _dbContext.Entry(transaction).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTransaction(PersonalTransaction transaction)
    {
        _dbContext.PersonalTransactions.Remove(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PersonalTransaction?> GetTransactionById(int id)
    {
        return await _dbContext.PersonalTransactions
            .Include(pt => pt.User)
            .Include(pt => pt.PersonalCategory)
            .FirstOrDefaultAsync(pt => pt.Id == id);
    }

    public async Task<List<PersonalTransaction>> GetTransactionsForPeriod(string userId, DateOnly startDate,
        DateOnly endDate)
    {
        return await _dbContext.PersonalTransactions
            .Include(pt => pt.User)
            .Include(pt => pt.PersonalCategory)
            .Where(pt =>
                pt.User.Id == userId &&
                pt.TransactionDate >= startDate &&
                pt.TransactionDate <= endDate)
            .Include(pt => pt.PersonalCategory)
            .ToListAsync();
    }
}