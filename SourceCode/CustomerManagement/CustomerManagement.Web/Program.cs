using CustomerManagement.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using CustomerManagement.Web.Services.Implementations;
using CustomerManagement.Web.Services.Interfaces;
using Blazored.LocalStorage;
using CustomerManagement.Web.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using CustomerManagement.Web.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider>(
        sp => sp.GetRequiredService<
            CustomAuthenticationStateProvider>());

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException(
        "ApiBaseUrl is not configured.");

builder.Services.AddScoped<JwtAuthorizationHandler>();
builder.Services
    .AddHttpClient("Api", client =>
    {
        client.BaseAddress = new Uri(apiBaseUrl);
    })
    .AddHttpMessageHandler<JwtAuthorizationHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("Api"));

builder.Services.AddMudServices();

await builder.Build().RunAsync();