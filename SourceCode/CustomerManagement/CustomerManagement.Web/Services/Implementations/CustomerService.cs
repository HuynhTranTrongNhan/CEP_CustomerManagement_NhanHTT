using System.Net.Http.Json;
using CustomerManagement.Web.Models.Common;
using CustomerManagement.Web.Models.Customer;
using CustomerManagement.Web.Services.Interfaces;

namespace CustomerManagement.Web.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;

    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedResult<CustomerModel>> GetCustomersAsync(
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var url =
            $"api/Customers?pageNumber={pageNumber}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(search))
        {
            url += $"&search={Uri.EscapeDataString(search.Trim())}";
        }

        var response =
            await _httpClient.GetFromJsonAsync<
                ApiResponse<PagedResult<CustomerModel>>>(url);

        if (response is null ||
            !response.Success ||
            response.Data is null)
        {
            return new PagedResult<CustomerModel>();
        }

        return response.Data;
    }
}