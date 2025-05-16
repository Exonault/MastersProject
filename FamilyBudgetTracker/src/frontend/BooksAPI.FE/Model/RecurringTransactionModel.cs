using System.ComponentModel.DataAnnotations;
using BooksAPI.FE.Attribute;

namespace BooksAPI.FE.Model;

public class RecurringTransactionModel
{
    [Required(ErrorMessage = "Amount is required")]
    [GreaterThanZero(ErrorMessage = "Amount should be greater than zero")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; }
    
    [Required(ErrorMessage = "Start date is required")]
    public DateTime? StartDate { get; set; }
    
    [Required(ErrorMessage = "End date is required")]
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; }
}