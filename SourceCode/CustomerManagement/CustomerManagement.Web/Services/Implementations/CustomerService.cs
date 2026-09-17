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
            url +=
                $"&search={Uri.EscapeDataString(search.Trim())}";
        }

        var response =
            await _httpClient.GetFromJsonAsync<
                ApiResponse<PagedResult<CustomerModel>>>(url);

        return response?.Data ?? new PagedResult<CustomerModel>();
    }

    public async Task<CustomerModel?> GetByIdAsync(int id)
    {
        var response =
            await _httpClient.GetFromJsonAsync<
                ApiResponse<CustomerModel>>(
                    $"api/Customers/{id}");

        return response?.Data;
    }

    public async Task<CustomerModel?> CreateAsync(
        CreateCustomerRequest request)
    {
        var httpResponse =
            await _httpClient.PostAsJsonAsync(
                "api/Customers",
                request);

        if (!httpResponse.IsSuccessStatusCode)
            return null;

        var response =
            await httpResponse.Content
                .ReadFromJsonAsync<ApiResponse<CustomerModel>>();

        return response?.Data;
    }

    public async Task<CustomerModel?> UpdateAsync(
        int id,
        UpdateCustomerRequest request)
    {
        var httpResponse =
            await _httpClient.PutAsJsonAsync(
                $"api/Customers/{id}",
                request);

        if (!httpResponse.IsSuccessStatusCode)
            return null;

        var response =
            await httpResponse.Content
                .ReadFromJsonAsync<ApiResponse<CustomerModel>>();

        return response?.Data;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response =
            await _httpClient.DeleteAsync(
                $"api/Customers/{id}");

        return response.IsSuccessStatusCode;
    }
}