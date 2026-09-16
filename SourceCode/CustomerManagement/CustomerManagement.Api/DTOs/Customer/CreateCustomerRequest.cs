using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CustomerManagement.Api.Validation;

namespace CustomerManagement.Api.DTOs.Customer;

public class CreateCustomerRequest
{
    //[Required]
    //[MaxLength(50)]
    //public string CustomerCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    [Required]
    [MaxLength(20)]
    [RegularExpression(
        @"^[0-9]+$",
        ErrorMessage = "Phone number must contain digits only.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [NotFutureDate]
    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; } = true;
}