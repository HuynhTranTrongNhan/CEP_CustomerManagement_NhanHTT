using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerManagement.Api.Configuration;
using CustomerManagement.Api.DTOs.Auth;
using CustomerManagement.Api.Services.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CustomerManagement.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(
        AppDbContext context,
        IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var username = request.Username.Trim();

        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Username == username &&
                x.IsActive);

        if (user is null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var token = GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(
            _jwtSettings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}