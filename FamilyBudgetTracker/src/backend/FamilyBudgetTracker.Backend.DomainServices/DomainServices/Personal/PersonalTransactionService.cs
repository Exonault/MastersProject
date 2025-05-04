using FamilyBudgetTracker.Backend.Data.Mappers.Personal;
using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Personal;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using FamilyBudgetTracker.Backend.Domain.Services.Personal;
using FamilyBudgetTracker.Shared.Contracts.Personal.Transaction;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Personal;

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

        user = user.ValidateUser();

        Category? category = await _categoryRepository.GetCategoryById(request.CategoryId);

        category = category.ValidateCategory(user.Id);

        PersonalTransaction transaction = request.ToPersonalTransaction();
        transaction.User = user;
        transaction.Category = category;

        await _transactionRepository.CreateTransaction(transaction);
    }

    public async Task UpdateTransaction(int id, UpdatePersonalTransactionRequest request, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Category? category = await _categoryRepository.GetCategoryById(request.CategoryId);

        category = category.ValidateCategory(user.Id);

        PersonalTransaction? transaction = await _transactionRepository.GetTransactionById(id);

        transaction = transaction.ValidatePersonalTransaction(user.Id);

        PersonalTransaction updatedTransaction = request.ToPersonalTransaction(transaction);

        await _transactionRepository.UpdateTransaction(updatedTransaction);
    }

    public async Task DeleteTransaction(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        PersonalTransaction? transaction = await _transactionRepository.GetTransactionById(id);

        transaction = transaction.ValidatePersonalTransaction(user.Id);

        await _transactionRepository.DeleteTransaction(transaction);
    }

    public async Task<PersonalTransactionResponse> GetTransactionById(int id, string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        PersonalTransaction? transaction = await _transactionRepository.GetTransactionById(id);

        transaction = transaction.ValidatePersonalTransaction(user.Id);

        PersonalTransactionResponse response = transaction.ToPersonalTransactionResponse();

        return response;
    }

    public async Task<List<PersonalTransactionResponse>> GetTransactionForPeriod(DateOnly startDate, DateOnly endDate,
        string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        List<PersonalTransaction> transactions =
            await _transactionRepository.GetTransactionForPeriod(user.Id, startDate, endDate);

        List<PersonalTransactionResponse> response = transactions.Select(x => x.ToPersonalTransactionResponse())
            .ToList();

        return response;
    }

    public async Task<TransactionForPeriodSummaryResponse> GetTransactionForPeriodSummary(DateOnly startDate, DateOnly endDate,
        string userId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        List<PersonalTransaction> transactions =
            await _transactionRepository.GetTransactionForPeriod(user.Id, startDate, endDate);

        var response = GenerateTransactionForPeriodSummaryResponse(transactions);

        return response;
    }


    private static TransactionForPeriodSummaryResponse GenerateTransactionForPeriodSummaryResponse(List<PersonalTransaction> transactions)
    {
        var expenseSum = 0.0m;
        var incomeSum = 0.0m;
        var expenseCategoryAmounts = new Dictionary<string, decimal>();
        foreach (var transaction in transactions)
        {
            var transactionCategory = transaction.Category;
            var transactionAmount = transaction.Amount;

            if (transactionCategory.Type == CategoryType.Income)
            {
                incomeSum += transactionAmount;
            }
            else if (transactionCategory.Type == CategoryType.Expense)
            {
                expenseSum += transactionAmount;
                if (!expenseCategoryAmounts.TryAdd(transactionCategory.Name, transactionAmount))
                {
                    expenseCategoryAmounts[transactionCategory.Name] += transactionAmount;
                }
            }
        }

        var result = new TransactionForPeriodSummaryResponse
        {
            IncomeAmount = incomeSum,
            ExpenseAmount = expenseSum,
            ExpenseCategoryAmounts = expenseCategoryAmounts
        };

        return result;
    }
}