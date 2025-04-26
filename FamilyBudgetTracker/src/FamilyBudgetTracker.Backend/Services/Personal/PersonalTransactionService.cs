using FamilyBudgetTracker.Backend.Mappers.Personal;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.BE.Commons.Contracts.Personal.Transaction;
using FamilyBudgetTracker.BE.Commons.Entities;
using FamilyBudgetTracker.BE.Commons.Entities.Personal;
using FamilyBudgetTracker.BE.Commons.Exceptions;
using FamilyBudgetTracker.BE.Commons.Repositories;
using FamilyBudgetTracker.BE.Commons.Repositories.Personal;
using FamilyBudgetTracker.BE.Commons.Services.Personal;
using FluentValidation;

namespace FamilyBudgetTracker.Backend.Services.Personal;

public class PersonalTransactionService : IPersonalTransactionService
{
    private readonly IPersonalTransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;


    public PersonalTransactionService(IPersonalTransactionRepository transactionRepository,
        IUserRepository userRepository, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task CreateTransaction(CreatePersonalTransactionRequest request, string userId)
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

        if (category.User.Id != userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
        }

        PersonalTransaction transaction = request.ToPersonalTransaction();

        transaction.User = user;
        transaction.Category = category;

        await _transactionRepository.CreateTransaction(transaction);
    }

    public async Task UpdateTransaction(int id, UpdatePersonalTransactionRequest request, string userId)
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

        if (category.User.Id != userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
        }

        PersonalTransaction? transaction = await _transactionRepository.GetTransactionById(id);

        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new InvalidOperationException(PersonalTransactionMessages.TransactionIsNotFromTheUser);
        }

        PersonalTransaction updatedTransaction = request.ToPersonalTransaction(transaction);

        updatedTransaction.Id = transaction.Id;
        updatedTransaction.Category = category;
        updatedTransaction.User = user;

        await _transactionRepository.UpdateTransaction(updatedTransaction);
    }

    public async Task DeleteTransaction(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        PersonalTransaction? transaction = await _transactionRepository.GetTransactionById(id);

        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new InvalidOperationException(PersonalTransactionMessages.DeleteImpossible);
        }

        await _transactionRepository.DeleteTransaction(transaction);
    }

    public async Task<PersonalTransactionResponse> GetTransactionById(int id, string userId)
    {
        PersonalTransaction? transaction = await _transactionRepository.GetTransactionById(id);

        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionMessages.NoTransactionFound);
        }

        if (transaction.User.Id != userId)
        {
            throw new InvalidOperationException(PersonalTransactionMessages.TransactionIsNotFromTheUser);
        }

        PersonalTransactionResponse response = transaction.ToPersonalTransactionResponse();

        return response;
    }

    public async Task<List<PersonalTransactionResponse>> GetTransactionForPeriod(DateOnly startDate, DateOnly endDate,
        string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        List<PersonalTransaction> transactions =
            await _transactionRepository.GetTransactionForPeriod(userId, startDate, endDate);

        List<PersonalTransactionResponse> response = transactions.Select(x => x.ToPersonalTransactionResponse())
            .ToList();

        return response;
    }
}