using CustomerManagement.Web.Models.Auth;

namespace CustomerManagement.Web.Services.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginRequest request);

    Task LogoutAsync();

    Task<string?> GetTokenAsync();
}