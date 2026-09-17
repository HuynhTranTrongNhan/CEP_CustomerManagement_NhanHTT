using CustomerManagement.Web.Models.Common;
using CustomerManagement.Web.Models.Customer;

namespace CustomerManagement.Web.Services.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerModel>> GetCustomersAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task<CustomerModel?> GetByIdAsync(int id);

    Task<CustomerModel?> CreateAsync(CreateCustomerRequest request);

    Task<CustomerModel?> UpdateAsync(int id, UpdateCustomerRequest request);

    Task<bool> DeleteAsync(int id);

    Task<CustomerSummaryModel?> GetSummaryAsync();
}