using CustomerManagement.Api.DTOs.Common;
using CustomerManagement.Api.DTOs.Customer;
using CustomerManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/customers
    // GET: api/customers?search=nguyen&pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<CustomerResponseDto>>>> GetAll([FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _customerService.GetAllAsync(
            search,
            pageNumber,
            pageSize);

        return Ok(
            ApiResponse<PagedResult<CustomerResponseDto>>.Ok(
                result,
                "Customers retrieved successfully."));
    }

    // GET: api/customers/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound(
                ApiResponse<CustomerResponseDto>.Fail(
                    "Customer not found."));
        }

        return Ok(
            ApiResponse<CustomerResponseDto>.Ok(
                customer,
                "Customer retrieved successfully."));
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> Create([FromBody] CreateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = customer.Id },
                ApiResponse<CustomerResponseDto>.Ok(
                    customer,
                    "Customer created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                ApiResponse<CustomerResponseDto>.Fail(
                    ex.Message));
        }
    }

    // PUT: api/customers/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.UpdateAsync(
                id,
                request);

            if (customer is null)
            {
                return NotFound(
                    ApiResponse<CustomerResponseDto>.Fail(
                        "Customer not found."));
            }

            return Ok(
                ApiResponse<CustomerResponseDto>.Ok(
                    customer,
                    "Customer updated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(
                ApiResponse<CustomerResponseDto>.Fail(
                    ex.Message));
        }
    }

    // DELETE: api/customers/1
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var deleted = await _customerService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(
                ApiResponse<object>.Fail(
                    "Customer not found."));
        }

        return Ok(
            ApiResponse<object>.Ok(
                null!,
                "Customer deleted successfully."));
    }
}