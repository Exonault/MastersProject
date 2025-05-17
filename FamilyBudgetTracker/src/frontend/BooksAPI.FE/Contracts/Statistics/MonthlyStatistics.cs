using System.Text.Json.Serialization;

namespace BooksAPI.FE.Contracts.Statistics;

public class MonthlyStatistics
{
    [JsonPropertyName("month")]
    public int Month { get; set; }
    
    [JsonPropertyName("income")]
    public decimal Income { get; set; }
    
    [JsonPropertyName("expense")]
    public decimal Expense { get; set; }
}