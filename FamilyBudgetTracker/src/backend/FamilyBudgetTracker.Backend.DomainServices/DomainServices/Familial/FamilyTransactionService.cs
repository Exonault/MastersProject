using FamilyBudgetTracker.Backend.Data.Mappers.Familial;
using FamilyBudgetTracker.Backend.Data.Validation;
using FamilyBudgetTracker.Backend.Domain.Entities;
using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Entities.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using FamilyBudgetTracker.Backend.Domain.Services.Familial;
using FamilyBudgetTracker.Shared.Contracts.Familial.FamilyTransaction;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Familial;

public class FamilyTransactionService : IFamilyTransactionService
{
    private readonly IFamilyTransactionRepository _familyTransactionRepository;
    private readonly IFamilyRepository _familyRepository;
    private readonly IFamilyCategoryRepository _familyCategoryRepository;
    private readonly IUserRepository _userRepository;


    public FamilyTransactionService(IFamilyTransactionRepository familyTransactionRepository, IFamilyRepository familyRepository,
        IFamilyCategoryRepository familyCategoryRepository, IUserRepository userRepository)
    {
        _familyTransactionRepository = familyTransactionRepository;
        _familyRepository = familyRepository;
        _familyCategoryRepository = familyCategoryRepository;
        _userRepository = userRepository;
    }

    public async Task CreateFamilyTransaction(FamilyTransactionRequest request, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetFamilyCategoryById(request.FamilyCategoryId);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id);

        FamilyTransaction familyTransaction = request.ToFamilyTransaction();

        familyTransaction.User = user;
        familyTransaction.Family = family;
        familyTransaction.Category = familyCategory;

        await _familyTransactionRepository.CreateFamilyTransaction(familyTransaction);
    }

    public async Task UpdateFamilyTransaction(int id, FamilyTransactionRequest request, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyCategory? familyCategory = await _familyCategoryRepository.GetFamilyCategoryById(request.FamilyCategoryId);

        familyCategory = familyCategory.ValidateFamilyCategory(family.Id);

        FamilyTransaction? familyTransaction = await _familyTransactionRepository.GetFamilyTransactionById(id);

        familyTransaction = familyTransaction.ValidateFamilyTransaction(family.Id, user.Id);
        familyTransaction = familyTransaction.ValidateFamilyTransactionCategory(familyCategory.Id);

        FamilyTransaction updatedTransaction = request.ToFamilyTransaction(familyTransaction);

        await _familyTransactionRepository.UpdateFamilyTransaction(updatedTransaction);
    }

    public async Task DeleteFamilyTransaction(int id, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyTransaction? familyTransaction = await _familyTransactionRepository.GetFamilyTransactionById(id);

        familyTransaction = familyTransaction.ValidateFamilyTransaction(family.Id, user.Id);

        await _familyTransactionRepository.DeleteFamilyTransaction(familyTransaction);
    }

    public async Task<FamilyTransactionResponse> GetFamilyTransactionById(int id, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        string familyRole = await _userRepository.GetMainFamilyRole(user);

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        FamilyTransaction? familyTransaction = await _familyTransactionRepository.GetFamilyTransactionById(id);

        familyTransaction = familyTransaction.ValidateFamilyTransaction(family.Id, user.Id);

        FamilyTransactionResponse response = familyTransaction.ToFamilyTransactionResponse(familyRole);

        return response;
    }

    public async Task<List<FamilyTransactionResponse>> GetFamilyTransactionsForPeriod(DateOnly startDate, DateOnly endDate, string userId,
        string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        List<FamilyTransaction> transactions =
            await _familyTransactionRepository.GetFamilyTransactionsForPeriod(family.Id, startDate, endDate);


        List<FamilyTransactionResponse> response = [];
        foreach (var transaction in transactions)
        {
            var mainFamilyRole = await _userRepository.GetMainFamilyRole(transaction.User);
            response.Add(transaction.ToFamilyTransactionResponse(mainFamilyRole));
        }

        return response;
    }

    public async Task<FamilyTransactionsForPeriodSummaryResponse> GetFamilyTransactionsForPeriodSummary(DateOnly startDate,
        DateOnly endDate, string userId, string familyId)
    {
        User? user = await _userRepository.GetById(userId);

        user = user.ValidateUser();

        Family? family = await _familyRepository.GetFamilyById(familyId);

        family = family.ValidateFamily();

        user.ValidateUserFamily(family.Id);

        List<FamilyTransaction> transactions =
            await _familyTransactionRepository.GetFamilyTransactionsForPeriod(family.Id, startDate, endDate);

        FamilyTransactionsForPeriodSummaryResponse response = GenerateFamilyTransactionForPeriodSummaryResponse(transactions);

        return response;
    }

    private static FamilyTransactionsForPeriodSummaryResponse GenerateFamilyTransactionForPeriodSummaryResponse(
        List<FamilyTransaction> transactions)
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

        return new FamilyTransactionsForPeriodSummaryResponse
        {
            TotalIncomeAmount = incomeSum,
            TotalExpenseAmount = expenseSum,
            ExpenseCategoryAmounts = expenseCategoryAmounts
        };
    }
}