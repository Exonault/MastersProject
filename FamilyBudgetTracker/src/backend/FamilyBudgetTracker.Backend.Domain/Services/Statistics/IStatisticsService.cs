using FamilyBudgetTracker.Shared.Contracts.Statistics;

namespace FamilyBudgetTracker.Backend.Domain.Services.Statistics;

public interface IStatisticsService
{
    Task<YearlyStatisticsResponse> GetYearlyStatistics(int year, string userId);

    Task<FamilyYearlyStatisticsResponse> GetFamilyYearlyStatistics(int year, string familyId, string userId);
}