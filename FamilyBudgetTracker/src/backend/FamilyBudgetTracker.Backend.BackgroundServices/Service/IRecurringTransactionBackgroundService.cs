namespace FamilyBudgetTracker.Backend.BackgroundServices.Service;

public interface IRecurringTransactionBackgroundService
{
    Task ProduceTransaction();
}