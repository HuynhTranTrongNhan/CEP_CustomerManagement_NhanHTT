using CustomerManagement.Api.DTOs.Common;
using CustomerManagement.Api.DTOs.Customer;

namespace CustomerManagement.Api.Services.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerResponseDto>> GetAllAsync(
        string? search,
        int pageNumber,
        int pageSize);

    Task<CustomerResponseDto?> GetByIdAsync(int id);

    Task<CustomerResponseDto> CreateAsync(
        CreateCustomerRequest request);

    Task<CustomerResponseDto?> UpdateAsync(
        int id,
        UpdateCustomerRequest request);

    Task<bool> DeleteAsync(int id);
}