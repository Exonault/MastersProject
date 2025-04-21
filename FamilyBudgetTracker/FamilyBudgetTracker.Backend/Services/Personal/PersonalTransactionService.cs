using FamilyBudgetTracker.Backend.Mappers.Personal;
using FamilyBudgetTracker.Backend.Messages;
using FamilyBudgetTracker.Backend.Messages.Personal;
using FamilyBudgetTracker.Entities.Contracts.Personal.Transaction;
using FamilyBudgetTracker.Entities.Entities;
using FamilyBudgetTracker.Entities.Entities.Personal;
using FamilyBudgetTracker.Entities.Exceptions;
using FamilyBudgetTracker.Entities.Repositories;
using FamilyBudgetTracker.Entities.Repositories.Personal;
using FamilyBudgetTracker.Entities.Services.Personal;
using FluentValidation;
using FluentValidation.Results;

namespace FamilyBudgetTracker.Backend.Services.Personal;

public class PersonalTransactionService : IPersonalTransactionService
{
    private readonly IPersonalTransactionRepository _transactionRepository;
    // private readonly IValidator<CreatePersonalTransactionRequest> _createTransactionValidator;
    // private readonly IValidator<UpdatePersonalTransactionRequest> _updateTransactionValidator;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;


    public PersonalTransactionService(IPersonalTransactionRepository transactionRepository,
        IValidator<CreatePersonalTransactionRequest> createTransactionValidator,
        IValidator<UpdatePersonalTransactionRequest> updateTransactionValidator, IUserRepository userRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        // _createTransactionValidator = createTransactionValidator;
        // _updateTransactionValidator = updateTransactionValidator;
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

        if (category.User.Id == userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
        }

        // ValidationResult validationResult = await _createTransactionValidator.ValidateAsync(request);
        //
        // if (!validationResult.IsValid)
        // {
        //     throw new ValidationException(validationResult.Errors);
        // }

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

        if (category.User.Id == userId)
        {
            throw new InvalidOperationException(CategoryMessages.CategoryIsNotFromTheUser);
        }

        PersonalTransaction? transaction = await _transactionRepository.GetTransactionById(id);

        if (transaction is null)
        {
            throw new ResourceNotFoundException(PersonalTransactionMessages.NoTransactionFound);
        }
        //
        // ValidationResult validationResult = await _updateTransactionValidator.ValidateAsync(request);
        //
        // if (!validationResult.IsValid)
        // {
        //     throw new ValidationException(validationResult.Errors);
        // }

        PersonalTransaction updatedTransaction = request.ToPersonalTransaction();

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

    public async Task<List<PersonalTransactionResponse>> GetTransactionForPeriod(
        PersonalTransactionsForPeriodRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
        {
            throw new UserNotFoundException(UserMessages.ValidationMessages.UserNotFound);
        }

        List<PersonalTransaction> transactions =
            await _transactionRepository.GetTransactionForPeriod(userId, request.StartDate, request.EndDate);

        List<PersonalTransactionResponse> response = transactions.Select(x => x.ToPersonalTransactionResponse())
            .ToList();

        return response;
    }
}