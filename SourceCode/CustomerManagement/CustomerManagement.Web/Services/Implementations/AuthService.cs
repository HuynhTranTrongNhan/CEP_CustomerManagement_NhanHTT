using Blazored.LocalStorage;
using CustomerManagement.Web.Authentication;
using CustomerManagement.Web.Models.Auth;
using CustomerManagement.Web.Models.Common;
using CustomerManagement.Web.Services.Interfaces;
using System.Net.Http.Json;

namespace CustomerManagement.Web.Services.Implementations;

public class AuthService : IAuthService
{
    private const string TokenKey = "authToken";
    private readonly CustomAuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(
        HttpClient httpClient,
        ILocalStorageService localStorage,
        CustomAuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authenticationStateProvider =
            authenticationStateProvider;
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(
            "api/Auth/login",
            request);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return false;
        }

        var response =
            await httpResponse.Content
                .ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        if (response?.Success != true ||
            response.Data is null ||
            string.IsNullOrWhiteSpace(response.Data.Token))
        {
            return false;
        }

        await _localStorage.SetItemAsync(
            TokenKey,
            response.Data.Token);

        _authenticationStateProvider.NotifyUserAuthentication(response.Data.Token);

        return true;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);

        _authenticationStateProvider.NotifyUserLogout();
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(TokenKey);
    }
}