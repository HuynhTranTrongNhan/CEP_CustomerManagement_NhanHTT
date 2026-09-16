using CustomerManagement.Api.DTOs.Auth;

namespace CustomerManagement.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}