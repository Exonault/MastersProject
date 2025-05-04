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
        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTransaction(PersonalTransaction transaction)
    {
        _dbContext.Entry(transaction).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTransaction(PersonalTransaction transaction)
    {
        _dbContext.Transactions.Remove(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PersonalTransaction?> GetTransactionById(int id)
    {
        return await _dbContext.Transactions
            .Include(pt => pt.User)
            .Include(pt => pt.Category)
            .FirstOrDefaultAsync(pt => pt.Id == id);
    }

    public async Task<List<PersonalTransaction>> GetTransactionForPeriod(string userId, DateOnly startDate,
        DateOnly endDate)
    {
        return await _dbContext.Transactions
            .Include(pt => pt.User)
            .Include(pt => pt.Category)
            .Where(pt => pt.User.Id == userId &&
                         pt.TransactionDate >= startDate &&
                         pt.TransactionDate <= endDate)
            .Include(pt => pt.Category)
            .ToListAsync();
    }
}