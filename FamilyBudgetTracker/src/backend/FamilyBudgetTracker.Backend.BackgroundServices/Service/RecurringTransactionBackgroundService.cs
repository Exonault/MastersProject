using FamilyBudgetTracker.Backend.Data.Mappers.Personal;
using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using Microsoft.Extensions.Logging;

namespace FamilyBudgetTracker.Backend.BackgroundServices.Service;

public class RecurringTransactionBackgroundService : IRecurringTransactionBackgroundService
{
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly IPersonalTransactionRepository _personalTransactionRepository;
    private readonly ILogger<RecurringTransactionBackgroundService> _logger;

    public RecurringTransactionBackgroundService(IRecurringTransactionRepository recurringTransactionRepository,
        IPersonalTransactionRepository personalTransactionRepository, ILogger<RecurringTransactionBackgroundService> logger)
    {
        _recurringTransactionRepository = recurringTransactionRepository;
        _personalTransactionRepository = personalTransactionRepository;
        _logger = logger;
    }

    public async Task ProduceTransaction()
    {
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        List<RecurringTransaction> recurringTransactions =
            await _recurringTransactionRepository.GetRecurringTransactionsByExecutionDate(date);

        foreach (RecurringTransaction rt in recurringTransactions)
        {
            PersonalTransaction personalTransaction = rt.ToPersonalTransaction();

            await _personalTransactionRepository.CreateTransaction(personalTransaction);

            rt.NextExecutionDate = GetNextExecutionDate(rt);

            await _recurringTransactionRepository.UpdateRecurringTransaction(rt);

            _logger.LogInformation($"Transaction from recurring transaction with id: {rt.Id} has been created");
        }
    }

    private DateOnly GetNextExecutionDate(RecurringTransaction transaction)
    {
        DateOnly executionDate = transaction.NextExecutionDate;

        if (executionDate == DateOnly.MinValue)
        {
            executionDate = transaction.StartDate;
        }

        DateOnly result;

        switch (transaction.Type)
        {
            case RecurringType.Weekly:
                result = executionDate.AddDays(7);
                break;

            case RecurringType.BiWeekly:
                result = executionDate.AddDays(14);
                break;

            case RecurringType.Monthly:
                result = executionDate.AddMonths(1);
                break;

            default:
                result = executionDate;
                break;
        }

        return result;
    }
}