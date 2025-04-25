using FamilyBudgetTracker.Backend.Mappers.Personal;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.RecurringTransaction;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Personal;
using FamilyBudgetTracker.BE.Commons.Services;
using FamilyBudgetTracker.BE.Commons.Services.Personal;

namespace FamilyBudgetTracker.Backend.Services.Personal;

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

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        Category? category = await _categoryRepository.GetCategoryById(request.CategoryId);

        if (category is null)
        {
            throw new ResourceNotFoundException(CategoryMessages.NoCategoryFound);
        }

        if (category.User.Id == userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
        }

        RecurringTransaction transaction = request.ToRecurringTransaction();

        transaction.User = user;
        transaction.Category = category;

        await _recurringTransactionRepository.CreateRecurringTransaction(transaction);
    }

    public async Task UpdateRecurringTransaction(int id, UpdateRecurringTransactionRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        Category? category = await _categoryRepository.GetCategoryById(request.CategoryId);

        if (category is null)
        {
            throw new ResourceNotFoundException(CategoryMessages.NoCategoryFound);
        }

        if (category.User.Id == userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
        }

        RecurringTransaction? transaction = await _recurringTransactionRepository.GetRecurringTransactionById(id);

        if (transaction is null)
        {
            throw new ResourceNotFoundException(RecurringTransactionMessages.NoTransactionFound);
        }

        RecurringTransaction updatedTransaction = request.ToRecurringTransaction();

        updatedTransaction.Id = transaction.Id;
        updatedTransaction.Category = category;
        updatedTransaction.User = user;

        await _recurringTransactionRepository.UpdateRecurringTransaction(updatedTransaction);
    }

    public async Task DeleteRecurringTransaction(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        RecurringTransaction? transaction = await _recurringTransactionRepository.GetRecurringTransactionById(id);

        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new InvalidOperationException(PersonalTransactionMessages.DeleteImpossible);
        }

        await _recurringTransactionRepository.DeleteRecurringTransaction(transaction);
    }

    public async Task<RecurringTransactionResponse> GetRecurringTransactionById(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        RecurringTransaction? transaction = await _recurringTransactionRepository.GetRecurringTransactionById(id);

        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new InvalidOperationException(PersonalTransactionMessages.TransactionIsNotFromTheUser);
        }

        RecurringTransactionResponse response = transaction.ToRecurringTransactionResponse();

        return response;
    }

    public async Task<List<RecurringTransactionResponse>> GetRecurringTransactionsForUser(string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        List<RecurringTransaction> transactions =
            await _recurringTransactionRepository.GetRecurringTransactionsByUserid(userId);

        List<RecurringTransactionResponse> response = transactions.Select(x => x.ToRecurringTransactionResponse())
            .ToList();

        return response;
    }
}