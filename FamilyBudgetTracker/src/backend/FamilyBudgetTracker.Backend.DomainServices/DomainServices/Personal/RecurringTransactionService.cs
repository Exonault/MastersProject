using FamilyBudgetTracker.Backend.Data.Mappers.Personal;
using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using FamilyBudgetTracker.Backend.Domain.Services.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.RecurringTransaction;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Personal;

public class RecurringTransactionService : IRecurringTransactionService
{
    private readonly IUserRepository _userRepository;
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    //TODO for all: calculate the NextExecutionDate 
    public RecurringTransactionService(IUserRepository userRepository,
        IRecurringTransactionRepository recurringTransactionRepository, ICategoryRepository categoryRepository)
    {
        _userRepository = userRepository;
        _recurringTransactionRepository = recurringTransactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task CreateRecurringTransaction(CreateRecurringTransactionRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Category? category = await _categoryRepository.GetCategoryById(request.CategoryId);

        category = category.ValidateCategory(user.Id);

        RecurringTransaction transaction = request.ToRecurringTransaction();

        transaction.User = user;
        transaction.Category = category;

        transaction.NextExecutionDate = GetNextExecutionDate(transaction);

        await _recurringTransactionRepository.CreateRecurringTransaction(transaction);
    }

    public async Task UpdateRecurringTransaction(int id, UpdateRecurringTransactionRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Category? category = await _categoryRepository.GetCategoryById(request.CategoryId);

        category = category.ValidateCategory(user.Id);

        RecurringTransaction? transaction = await _recurringTransactionRepository.GetRecurringTransactionById(id);

        transaction = transaction.ValidateRecurringTransaction(user.Id);

        RecurringTransaction updatedTransaction = request.ToRecurringTransaction(transaction);

        await _recurringTransactionRepository.UpdateRecurringTransaction(updatedTransaction);
    }

    public async Task DeleteRecurringTransaction(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        RecurringTransaction? transaction = await _recurringTransactionRepository.GetRecurringTransactionById(id);

        transaction = transaction.ValidateRecurringTransaction(user.Id);

        await _recurringTransactionRepository.DeleteRecurringTransaction(transaction);
    }

    public async Task<RecurringTransactionResponse> GetRecurringTransactionById(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        RecurringTransaction? transaction = await _recurringTransactionRepository.GetRecurringTransactionById(id);

        transaction = transaction.ValidateRecurringTransaction(user.Id);

        RecurringTransactionResponse response = transaction.ToRecurringTransactionResponse();

        return response;
    }

    public async Task<List<RecurringTransactionResponse>> GetRecurringTransactionsForUser(string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        List<RecurringTransaction> transactions =
            await _recurringTransactionRepository.GetRecurringTransactionsByUserid(user.Id);

        List<RecurringTransactionResponse> response = transactions.Select(x => x.ToRecurringTransactionResponse())
            .ToList();

        return response;
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