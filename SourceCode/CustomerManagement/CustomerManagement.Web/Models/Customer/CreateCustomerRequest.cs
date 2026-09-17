using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Web.Models.Customer;

public class CreateCustomerRequest
{
    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
    [MaxLength(255)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    [MaxLength(20)]
    [RegularExpression(
        @"^[0-9]+$",
        ErrorMessage = "Số điện thoại chỉ được chứa chữ số.")]
    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; } = true;
}