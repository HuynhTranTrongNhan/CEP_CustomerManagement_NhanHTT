using CustomerManagement.Api.DTOs.Common;
using CustomerManagement.Api.DTOs.Customer;
using CustomerManagement.Api.Services.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Infrastructure.Repositories.Interfaces;

namespace CustomerManagement.Api.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PagedResult<CustomerResponseDto>> GetAllAsync(string? search, int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 10;
        }

        if (pageSize > 100)
        {
            pageSize = 100;
        }

        var customers = await _customerRepository.GetAllAsync(
            search,
            pageNumber,
            pageSize);

        var totalCount = await _customerRepository.GetCountAsync(
            search);

        var items = customers
            .Select(MapToDto)
            .ToList();

        return new PagedResult<CustomerResponseDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        return customer is null
            ? null
            : MapToDto(customer);
    }

    public async Task<CustomerResponseDto> CreateAsync(CreateCustomerRequest request)
    {
        var customerCode = request.CustomerCode.Trim();

        var exists =
            await _customerRepository.ExistsByCustomerCodeAsync(
                customerCode);

        if (exists)
        {
            throw new InvalidOperationException(
                "Customer code already exists.");
        }

        var customer = new Customer
        {
            CustomerCode = customerCode,
            FullName = request.FullName.Trim(),
            Email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            DateOfBirth = request.DateOfBirth,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<CustomerResponseDto?> UpdateAsync(int id, UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            return null;
        }

        var customerCode = request.CustomerCode.Trim();

        var exists =
            await _customerRepository.ExistsByCustomerCodeAsync(
                customerCode,
                id);

        if (exists)
        {
            throw new InvalidOperationException(
                "Customer code already exists.");
        }

        customer.CustomerCode = customerCode;
        customer.FullName = request.FullName.Trim();
        customer.Email = string.IsNullOrWhiteSpace(request.Email)
            ? null
            : request.Email.Trim();
        customer.PhoneNumber = request.PhoneNumber.Trim();
        customer.DateOfBirth = request.DateOfBirth;
        customer.IsActive = request.IsActive;
        customer.UpdatedAt = DateTime.UtcNow;

        _customerRepository.Update(customer);

        await _customerRepository.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer is null)
        {
            return false;
        }

        _customerRepository.Delete(customer);

        await _customerRepository.SaveChangesAsync();

        return true;
    }

    private static CustomerResponseDto MapToDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            CustomerCode = customer.CustomerCode,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            DateOfBirth = customer.DateOfBirth,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }
}