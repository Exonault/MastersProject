using BooksAPI.FE.Contracts.Personal.Category;
using BooksAPI.FE.Contracts.Personal.RecurringTransaction;
using BooksAPI.FE.Contracts.Personal.Transaction;
using BooksAPI.FE.Contracts.Statistics;
using MudBlazor;

namespace BooksAPI.FE.Util;

public static class MockUtil
{
    public static List<CategoryResponse> GetMockCategoryResponse()
    {
        List<CategoryResponse> response = new List<CategoryResponse>();

        response.Add(new CategoryResponse
        {
            Id = 1,
            Name = "Salary",
            Icon = Icons.Material.Filled.Home,
            Type = "Income",
            Limit = null
        });

        response.Add(new CategoryResponse
        {
            Id = 2,
            Name = "Bills",
            Icon = "bi bi-buildings",
            Type = "Expense",
            Limit = null
        });

        response.Add(new CategoryResponse
        {
            Id = 3,
            Name = "General",
            Icon = "bi bi bi-tag",
            Type = "Expense",
            Limit = null
        });

        response.Add(new CategoryResponse
        {
            Id = 4,
            Name = "Dine out",
            Icon = "bi bi bi-cup-straw",
            Type = "Expense",
            Limit = 300.0m
        });

        response.Add(new CategoryResponse
        {
            Id = 5,
            Name = "Groceries",
            Icon = "bi bi bi-bag-fill",
            Type = "Expense",
            Limit = null
        });

        return response;
    }

    public static List<PersonalTransactionResponse> GetMockTransactionsResponse()
    {
        List<PersonalTransactionResponse> response = new List<PersonalTransactionResponse>();

        response.Add(new PersonalTransactionResponse
        {
            Id = 1,
            Amount = 1000.00m,
            Description = "Salary",
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 01),
            Category = new CategoryResponse
            {
                Id = 1,
                Name = "Salary",
                Icon = Icons.Material.Filled.Home,
                Type = "Income",
                Limit = 1000
            }
        });

        response.Add(new PersonalTransactionResponse
        {
            Id = 2,
            Amount = 200.00m,
            Description = "Bills",
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 01),
            Category = new CategoryResponse
            {
                Id = 2,
                Name = "Bills",
                Icon = "bi bi-credit-card",
                Type = "Income",
                Limit = 1000.00m
            }
        });

        response.Add(new PersonalTransactionResponse
        {
            Id = 3,
            Amount = 90.00m,
            Description = null,
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 04),
            Category = new CategoryResponse
            {
                Id = 3,
                Name = "General",
                Icon = "bi bi bi-tag",
                Type = "Income",
                Limit = null
            }
        });

        response.Add(new PersonalTransactionResponse
        {
            Id = 4,
            Amount = 26.00m,
            Description = null,
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 7),
            Category = new CategoryResponse
            {
                Id = 4,
                Name = "Dine out",
                Icon = "bi bi bi-cup-straw",
                Type = "Income",
                Limit = null
            }
        });

        response.Add(new PersonalTransactionResponse
        {
            Id = 5,
            Amount = 45.00m,
            Description = null,
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 10),
            Category = new CategoryResponse
            {
                Id = 5,
                Name = "Groceries",
                Icon = "bi bi bi-bag-fill",
                Type = "Income",
                Limit = null
            }
        });


        return response;
    }

    public static PersonalTransactionPeriodSummaryResponse GetMockTransactionSummaryResponse()
    {
        var expenses = new Dictionary<string, decimal>();
        expenses.Add("Bills", 200.00m);
        expenses.Add("General", 90.00m);
        expenses.Add("Dine out", 26.00m);
        expenses.Add("Groceries", 45.00m);

        return new PersonalTransactionPeriodSummaryResponse
        {
            IncomeAmount = 1000.00m,
            ExpenseAmount = expenses.Sum(x => x.Value),
            ExpenseCategoryAmounts = expenses
        };
    }


    public static List<RecurringTransactionResponse> GetRecurringTransactionsMockResponse()
    {
        List<RecurringTransactionResponse> response = new List<RecurringTransactionResponse>();

        response.Add(new RecurringTransactionResponse
        {
            Id = 1,
            Amount = 1000.00m,
            Description = "Salary",
            Type = "Monthly",
            StartDate = new DateOnly(2025, 01, 01),
            NextExecutionDate = default,
            EndDate = new DateOnly(2026, 01, 01),
            Category = new CategoryResponse
            {
                Id = 1,
                Name = "Salary",
                Icon = Icons.Material.Filled.Home,
                Type = "Income",
                Limit = null
            }
        });

        response.Add(new RecurringTransactionResponse
        {
            Id = 2,
            Amount = 200.00m,
            Description = "Bills",
            Type = "Monthly",
            StartDate = new DateOnly(2025, 01, 01),
            NextExecutionDate = default,
            EndDate = new DateOnly(2026, 01, 01),
            Category = new CategoryResponse
            {
                Id = 2,
                Name = "Bills",
                Icon = "bi bi-buildings",
                Type = "Income",
                Limit = null
            }
        });


        return response;
    }


    public static YearlyStatisticsResponse GetPersonalStatisticsResponseMock()
    {
        List<MonthlyStatistics> monthlyStatistics = Enumerable.Range(1, 12).Select(x => new MonthlyStatistics
            {
                Month = x,
                Income = 1000.00m,
                Expense = Random.Shared.Next(1, 10) * 0.5m * 100m
            })
            .ToList();
        List<PersonalTransactionResponse> mockTransactions = GetMockTransactionsResponse();

        List<CategoryStatistics> categoryStatistics = GetMockCategoryResponse().Select(x => new CategoryStatistics
        {
            CategoryId = x.Id,
            CategoryName = x.Name,
            TotalAmount = Random.Shared.Next(100, 500)
        }).ToList();


        return new YearlyStatisticsResponse
        {
            Year = 2025,
            TotalIncome = monthlyStatistics.Sum(x => x.Income),
            TotalExpense = monthlyStatistics.Sum(x => x.Expense),
            MonthlyStatistics = monthlyStatistics,
            CategoryStatistics = categoryStatistics
        };
    }
}