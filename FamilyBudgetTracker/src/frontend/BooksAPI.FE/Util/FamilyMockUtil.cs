using BooksAPI.FE.Contracts.Familial.FamilyCategory;
using BooksAPI.FE.Contracts.Familial.FamilyTransaction;
using BooksAPI.FE.Contracts.Statistics;
using BooksAPI.FE.Contracts.User;

namespace BooksAPI.FE.Util;

public static class FamilyMockUtil
{
    public static FamilyTransactionPeriodSummaryResponse GetFamilySummaryMockResponse()
    {
        var expenses = new Dictionary<string, decimal>();
        expenses.Add("Bills", 400.00m);
        expenses.Add("General", 30.00m);
        expenses.Add("Groceries", 150.00m);

        return new FamilyTransactionPeriodSummaryResponse
        {
            TotalIncomeAmount = 1320.00m,
            TotalExpenseAmount = expenses.Sum(x => x.Value),
            ExpenseCategoryAmounts = expenses
        };
    }


    public static List<FamilyCategoryResponse> GetFamilyCategoriesMockResponse()
    {
        List<FamilyCategoryResponse> response = new List<FamilyCategoryResponse>();

        response.Add(new FamilyCategoryResponse
        {
            Id = 1,
            Name = "Income",
            Icon = "bi bi-bank2",
            Type = "Income",
            Limit = null
        });

        response.Add(new FamilyCategoryResponse
        {
            Id = 2,
            Name = "Bills",
            Icon = "bi bi-building",
            Type = "Expense",
            Limit = null
        });

        response.Add(new FamilyCategoryResponse
        {
            Id = 3,
            Name = "General",
            Icon = "bi bi-calendar-fill",
            Type = "Expense",
            Limit = null
        });

        response.Add(new FamilyCategoryResponse
        {
            Id = 4,
            Name = "Groceries",
            Icon = "bi bi-bag-heart-fill",
            Type = "Expense",
            Limit = null
        });


        return response;
    }

    public static List<FamilyTransactionResponse> GetFamilyTransactionsMockResponse()
    {
        List<FamilyTransactionResponse> response = new List<FamilyTransactionResponse>();

        response.Add(new FamilyTransactionResponse
        {
            Id = 1,
            Amount = 1200.00m,
            Description = "Salary",
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 01),
            Category = new FamilyCategoryResponse
            {
                Id = 1,
                Name = "Income",
                Icon = "bi bi-bank2",
                Type = "Income",
                Limit = null
            },
            FamilyMember = new FamilyMemberResponse
            {
                Id = "string",
                UserName = "family",
                Email = "family@family.family",
                FamilyRole = "FamilyMember"
            }
        });

        response.Add(new FamilyTransactionResponse
        {
            Id = 2,
            Amount = 120.00m,
            Description = "Carry over from last month",
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 03),
            Category = new FamilyCategoryResponse
            {
                Id = 1,
                Name = "Income",
                Icon = "bi bi-bank2",
                Type = "Income",
                Limit = null
            },
            FamilyMember = new FamilyMemberResponse
            {
                Id = "string2",
                UserName = "familyMember2",
                Email = "familyMember2@family.family",
                FamilyRole = "FamilyMember"
            }
        });

        response.Add(new FamilyTransactionResponse
        {
            Id = 3,
            Amount = 300.00m,
            Description = "Bills",
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 01),
            Category = new FamilyCategoryResponse
            {
                Id = 2,
                Name = "Bills",
                Icon = "bi bi-building",
                Type = "Expense",
                Limit = null
            },
            FamilyMember = new FamilyMemberResponse
            {
                Id = "string",
                UserName = "family",
                Email = "family@family.family",
                FamilyRole = "FamilyMember"
            }
        });


        response.Add(new FamilyTransactionResponse
        {
            Id = 4,
            Amount = 30.00m,
            Description = "General",
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 09),
            Category = new FamilyCategoryResponse
            {
                Id = 3,
                Name = "General",
                Icon = "bi bi-calendar-fill",
                Type = "Expense",
                Limit = null
            },
            FamilyMember = new FamilyMemberResponse
            {
                Id = "string",
                UserName = "familyMember",
                Email = "familyMember@family.family",
                FamilyRole = "FamilyMember"
            }
        });


        response.Add(new FamilyTransactionResponse
        {
            Id = 5,
            Amount = 150.00m,
            Description = "Groceries for the week",
            TransactionDate = new DateOnly(2025, DateTime.Today.Month, 11),
            Category = new FamilyCategoryResponse
            {
                Id = 4,
                Name = "Groceries",
                Icon = "bi bi-bag-heart-fill",
                Type = "Expense",
                Limit = null
            },
            FamilyMember = new FamilyMemberResponse
            {
                Id = "string2",
                UserName = "familyMember2",
                Email = "family2@family.family",
                FamilyRole = "FamilyMember"
            }
        });

        return response;
    }

    public static FamilyYearlyStatisticsResponse GetFamilyStatisticsResponseMock(int year)
    {
        List<FamilyTransactionResponse> transactions = GetFamilyTransactionsMockResponse();

        List <FamilyTransactionResponse> expenseTransactions = transactions.Where(x => x.Category.Type == "Expense").ToList();
        
        List<MonthlyStatistics> monthlyStatistics;
        if (year == 2025)
        {
            monthlyStatistics = Enumerable.Range(1, 6).Select(x => new MonthlyStatistics
                {
                    Month = x,
                    Income = 1320.00m,
                    Expense = expenseTransactions.Sum(t => t.Amount) + Random.Shared.Next(100, 300)
                })
                .ToList();
        }
        else
        {
            monthlyStatistics = Enumerable.Range(1, 12).Select(x => new MonthlyStatistics
                {
                    Month = x,
                    Income = 1320.00m,
                    Expense = expenseTransactions.Sum(t => t.Amount) + Random.Shared.Next(100, 300)
                })
                .ToList();
        }

        List<CategoryStatistics> categoryStatistics = GetFamilyCategoriesMockResponse()
            .Select(x => new CategoryStatistics
            {
                CategoryId = x.Id,
                CategoryName = x.Name,
                TotalAmount = transactions
                    .Where(t => t.Category.Name == x.Name)
                    .Sum(t => t.Amount) * (year == 2025 ? 6 : 12) 
                              + (x.Type == "Income" ? 0 : Random.Shared.Next(100, 200))

            }).ToList();


        return new FamilyYearlyStatisticsResponse
        {
            Year = 2025,
            TotalIncome = monthlyStatistics.Sum(x => x.Income),
            TotalExpense = monthlyStatistics.Sum(x => x.Expense),
            MonthlyStatistics = monthlyStatistics,
            CategoryStatistics = categoryStatistics
        };
    }
}