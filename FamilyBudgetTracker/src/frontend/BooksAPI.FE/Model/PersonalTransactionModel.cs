using System.ComponentModel.DataAnnotations;
using BooksAPI.FE.Attribute;

namespace BooksAPI.FE.Model;

public class PersonalTransactionModel
{
    [Required(ErrorMessage = "Amount is required")]
    [GreaterThanZero(ErrorMessage = "Amount should be greater than zero")]
    public decimal Amount { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Transaction date is required")]
    [NotInFuture(ErrorMessage = "Transaction date cannot be in the future")]
    public DateTime? TransactionDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; }
}