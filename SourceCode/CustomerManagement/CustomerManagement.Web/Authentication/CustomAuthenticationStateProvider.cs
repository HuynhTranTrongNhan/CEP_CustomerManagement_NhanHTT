using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace CustomerManagement.Web.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";

    private readonly ILocalStorageService _localStorage;

    public CustomAuthenticationStateProvider(
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        var token =
            await _localStorage.GetItemAsync<string>(TokenKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return Anonymous();
        }

        var handler = new JwtSecurityTokenHandler();

        JwtSecurityToken jwt;

        try
        {
            jwt = handler.ReadJwtToken(token);
        }
        catch
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            return Anonymous();
        }

        if (jwt.ValidTo <= DateTime.UtcNow)
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            return Anonymous();
        }

        var identity = new ClaimsIdentity(
            jwt.Claims,
            authenticationType: "jwt",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public void NotifyUserAuthentication(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity(
            jwt.Claims,
            authenticationType: "jwt",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(Anonymous()));
    }

    private static AuthenticationState Anonymous()
    {
        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity()));
    }
}