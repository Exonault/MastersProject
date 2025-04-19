using FamilyBudgetTracker.Entities.Entities.Personal;

namespace FamilyBudgetTracker.Entities.Repositories.Personal;

public interface IPersonalTransactionRepository
{
    Task CreateTransaction(PersonalTransaction transaction);

    Task UpdateTransaction(PersonalTransaction transaction);

    Task DeleteTransaction(PersonalTransaction transaction);

    Task<PersonalTransaction?> GetTransactionById(int id);

    Task<List<PersonalTransaction>> GetTransactionForPeriod(string userId, DateOnly startDate, DateOnly endDate);
}