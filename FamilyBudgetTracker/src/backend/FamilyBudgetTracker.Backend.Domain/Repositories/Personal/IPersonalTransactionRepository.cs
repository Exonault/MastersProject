using FamilyBudgetTracker.Backend.Domain.Entities.Personal;

namespace FamilyBudgetTracker.Backend.Domain.Repositories.Personal;

public interface IPersonalTransactionRepository
{
    Task CreateTransaction(PersonalTransaction transaction);

    Task UpdateTransaction(PersonalTransaction transaction);

    Task DeleteTransaction(PersonalTransaction transaction);

    Task<PersonalTransaction?> GetTransactionById(int id);

    Task<List<PersonalTransaction>> GetTransactionsForPeriod(string userId, DateOnly startDate, DateOnly endDate);
}