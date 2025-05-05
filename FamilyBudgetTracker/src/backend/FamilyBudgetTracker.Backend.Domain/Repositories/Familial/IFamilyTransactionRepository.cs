using FamilyBudgetTracker.Backend.Domain.Entities.Familial;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Familial;

public interface IFamilyTransactionRepository
{
    Task CreateFamilyTransaction(FamilyTransaction transaction);

    Task UpdateFamilyTransaction(FamilyTransaction transaction);

    Task DeleteFamilyTransaction(FamilyTransaction transaction);

    Task<FamilyTransaction?> GetFamilyTransactionById(int id);
    
    Task<List<FamilyTransaction>> GetFamilyTransactionsForPeriod(Guid familyId, DateOnly startDate, DateOnly endDate);
}