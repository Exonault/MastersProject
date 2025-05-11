using System.ComponentModel.DataAnnotations;

namespace BooksAPI.FE.Attribute;

public class GreaterThanZeroAttribute:ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult(ErrorMessage ?? "Value is required.");
        }

        if (value is decimal decimalValue)
        {
            if (decimalValue <= 0)
            {
                return new ValidationResult(ErrorMessage ?? "Value must be greater than zero.");
            }
        }
        else if (value is int intValue)
        {
            if (intValue <= 0)
            {
                return new ValidationResult(ErrorMessage ?? "Value must be greater than zero.");
            }
        }
        else
        {
            return new ValidationResult(ErrorMessage ?? "Invalid data type.");
        }

        return ValidationResult.Success;
    }
}