using CustomerManagement.Api.DTOs.Auth;
using CustomerManagement.Api.DTOs.Common;
using CustomerManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result is null)
        {
            return Unauthorized(
                ApiResponse<LoginResponse>.Fail(
                    "Invalid username or password."));
        }

        return Ok(
            ApiResponse<LoginResponse>.Ok(
                result,
                "Login successful."));
    }
}