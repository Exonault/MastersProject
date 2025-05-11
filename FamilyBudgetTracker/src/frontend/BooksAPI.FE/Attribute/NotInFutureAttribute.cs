using System.ComponentModel.DataAnnotations;

namespace BooksAPI.FE.Attribute;

public class NotInFutureAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
            {
                return new ValidationResult(this.ErrorMessage);
            } 
        }

        return ValidationResult.Success;
    }
}