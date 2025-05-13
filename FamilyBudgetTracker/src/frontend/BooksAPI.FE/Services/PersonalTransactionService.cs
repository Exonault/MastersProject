using BooksAPI.FE.Interfaces;

namespace BooksAPI.FE.Services;

public class PersonalTransactionService : IPersonalTransactionService
{
 
    
    
    private static (DateOnly Start, DateOnly End) GetMonthRange(DateOnly date)
    {
        var start = new DateOnly(date.Year, date.Month, 1);
        var end = new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        return (start, end);
    }
}