using CustomerManagement.Api.DTOs.Common;
using CustomerManagement.Api.DTOs.Customer;
using CustomerManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> Create([FromBody] CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            ApiResponse<CustomerResponseDto>.Ok(
                customer,
                "Customer created successfully."));
    }

    // PUT: api/customers/1
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> Update(int id, [FromBody] UpdateCustomerRequest request)
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

    // DELETE: api/customers/1
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
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

    [HttpGet("summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _customerService.GetSummaryAsync();

        return Ok(new ApiResponse<CustomerSummaryDto>
        {
            Success = true,
            Message = "Get customer summary successfully.",
            Data = result
        });
    }
}