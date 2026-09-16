using CustomerManagement.Web.Models.Common;
using CustomerManagement.Web.Models.Customer;

namespace CustomerManagement.Web.Services.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerModel>> GetCustomersAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10);
}