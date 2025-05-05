using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetTracker.Backend.Data.Repositories.Familial;

public class FamilyTransactionRepository : IFamilyTransactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FamilyTransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateFamilyTransaction(FamilyTransaction transaction)
    {
        await _dbContext.FamilyTransactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateFamilyTransaction(FamilyTransaction transaction)
    {
        _dbContext.Entry(transaction).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFamilyTransaction(FamilyTransaction transaction)
    {
        _dbContext.FamilyTransactions.Remove(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FamilyTransaction?> GetFamilyTransactionById(int id)
    {
        return await _dbContext.FamilyTransactions
            .Include(ft => ft.Family)
            .Include(ft => ft.User)
            .Include(ft => ft.Category)
            .FirstOrDefaultAsync(ft => ft.Id == id);
    }

    public async Task<List<FamilyTransaction>> GetFamilyTransactionsForPeriod(Guid familyId, DateOnly startDate, DateOnly endDate)
    {
        return await _dbContext.FamilyTransactions
            .Include(ft => ft.Family)
            .Include(ft => ft.User)
            .Include(ft => ft.Category)
            .Where(pt =>
                pt.Family.Id == familyId &&
                pt.TransactionDate >= startDate &&
                pt.TransactionDate <= endDate)
            .ToListAsync();
    }
}