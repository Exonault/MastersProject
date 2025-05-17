using System.Text.Json.Serialization;

namespace FamilyBudgetTracker.Shared.Contracts.Statistics;

public class FamilyYearlyStatisticsResponse
{
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("totalIncome")]
    public decimal TotalIncome{ get; set; }

    [JsonPropertyName("toalExpense")]
    public decimal TotalExpense { get; set; }

    [JsonPropertyName("monthlyStatistics")]
    public List<MonthlyStatistics> MonthlyStatistics { get; set; }

    [JsonPropertyName("categoryStatistics")]
    public List<CategoryStatistics> CategoryStatistics { get; set; }
}