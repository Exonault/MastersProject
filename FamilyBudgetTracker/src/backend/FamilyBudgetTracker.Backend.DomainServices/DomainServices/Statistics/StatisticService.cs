using FamilyBudgetTracker.Backend.Domain.Entities.Common;
using FamilyBudgetTracker.Backend.Domain.Repositories.Familial;
using FamilyBudgetTracker.Backend.Domain.Repositories.Personal;
using FamilyBudgetTracker.Backend.Domain.Services.Statistics;
using FamilyBudgetTracker.Shared.Contracts.Statistics;

namespace FamilyBudgetTracker.Backend.DomainServices.DomainServices.Statistics;

public class StatisticService : IStatisticsService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPersonalTransactionRepository _personalTransactionRepository;

    private readonly IFamilyCategoryRepository _familyCategoryRepository;
    private readonly IFamilyTransactionRepository _familyTransactionRepository;


    public StatisticService(ICategoryRepository categoryRepository, IPersonalTransactionRepository personalTransactionRepository,
        IFamilyCategoryRepository familyCategoryRepository, IFamilyTransactionRepository familyTransactionRepository)
    {
        _categoryRepository = categoryRepository;
        _personalTransactionRepository = personalTransactionRepository;
        _familyCategoryRepository = familyCategoryRepository;
        _familyTransactionRepository = familyTransactionRepository;
    }

    public async Task<YearlyStatisticsResponse> GetYearlyStatistics(int year, string userId)
    {
        DateOnly startDate = new DateOnly(year, 1, 1);
        DateOnly endDate = new DateOnly(year, 12, 31);

        var transactions = await _personalTransactionRepository.GetTransactionsForPeriod(userId, startDate, endDate);

        var categories = await _categoryRepository.GetAllCategoriesForUser(userId);

        var monthlyStatistics = Enumerable.Range(1, 12)
            .Select(month => new MonthlyStatistics
            {
                Month = month,
                Income = transactions
                    .Where(t => t.TransactionDate.Month == month && t.Category.Type == CategoryType.Income)
                    .Sum(t => t.Amount),
                Expense = transactions
                    .Where(t => t.TransactionDate.Month == month && t.Category.Type == CategoryType.Expense)
                    .Sum(t => t.Amount)
            })
            .ToList();

        var categoryStatistics = categories
            .Select(category => new CategoryStatistics
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                TotalAmount = transactions
                    .Where(t => t.Category.Id == category.Id)
                    .Sum(t => t.Amount)
            })
            .ToList();

        var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        var totalExpenses = transactions.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount));

        return new YearlyStatisticsResponse
        {
            Year = year,
            TotalIncome = totalIncome,
            TotalExpense = totalExpenses,
            MonthlyStatistics = monthlyStatistics,
            CategoryStatistics = categoryStatistics
        };
    }


    public async Task<FamilyYearlyStatisticsResponse> GetFamilyYearlyStatistics(int year, string familyId, string userId)
    {
        DateOnly startDate = new DateOnly(year, 1, 1);
        DateOnly endDate = new DateOnly(year, 12, 31);

        var transactions = await _familyTransactionRepository.GetFamilyTransactionsForPeriod(Guid.Parse(familyId), startDate, endDate);

        var categories = await _familyCategoryRepository.GetFamilyCategoriesByFamilyId(familyId);

        var monthlyStatistics = Enumerable.Range(1, 12)
            .Select(month => new MonthlyStatistics
            {
                Month = month,
                Income = transactions
                    .Where(t => t.TransactionDate.Month == month && t.Category.Type == CategoryType.Income)
                    .Sum(t => t.Amount),
                Expense = transactions
                    .Where(t => t.TransactionDate.Month == month && t.Category.Type == CategoryType.Expense)
                    .Sum(t => t.Amount)
            })
            .ToList();

        var categoryStatistics = categories
            .Select(category => new CategoryStatistics
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                TotalAmount = transactions
                    .Where(t => t.Category.Id == category.Id)
                    .Sum(t => t.Amount)
            })
            .ToList();

        var totalIncome = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        var totalExpenses = transactions.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount));

        return new FamilyYearlyStatisticsResponse
        {
            Year = year,
            TotalIncome = totalIncome,
            TotalExpense = totalExpenses,
            MonthlyStatistics = monthlyStatistics,
            CategoryStatistics = categoryStatistics
        };
    }
}