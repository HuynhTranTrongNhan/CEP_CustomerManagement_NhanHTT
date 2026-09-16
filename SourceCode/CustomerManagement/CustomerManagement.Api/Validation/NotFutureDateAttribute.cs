using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Api.Validation;

public class NotFutureDateAttribute : ValidationAttribute
{
    public NotFutureDateAttribute()
    {
        ErrorMessage = "Date of birth cannot be in the future.";
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is DateTime date && date.Date > DateTime.UtcNow.Date)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}