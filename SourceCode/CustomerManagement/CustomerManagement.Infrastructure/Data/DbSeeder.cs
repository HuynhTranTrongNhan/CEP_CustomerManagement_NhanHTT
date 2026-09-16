using CustomerManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Đảm bảo database đã tồn tại.
        await context.Database.MigrateAsync();

        // Seed Users
        if (!await context.Users.AnyAsync())
        {
            var admin = new User
            {
                Username = "admin",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<User>();

            admin.PasswordHash = passwordHasher.HashPassword(
                admin,
                "Admin@123"
            );

            await context.Users.AddAsync(admin);
        }

        // Seed Customers
        if (!await context.Customers.AnyAsync())
        {
            var customers = new List<Customer>
            {
                new()
                {
                    CustomerCode = $"TMP-{Guid.NewGuid():N}",
                    FullName = "Nguyen Van An",
                    Email = "an@example.com",
                    PhoneNumber = "0901234567",
                    DateOfBirth = new DateTime(1995, 5, 10),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                new()
                {
                    CustomerCode = $"TMP-{Guid.NewGuid():N}",
                    FullName = "Tran Thi Binh",
                    Email = "binh@example.com",
                    PhoneNumber = "0912345678",
                    DateOfBirth = new DateTime(1997, 8, 15),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                new()
                {
                    CustomerCode = $"TMP-{Guid.NewGuid():N}",
                    FullName = "Le Van Cuong",
                    Email = "cuong@example.com",
                    PhoneNumber = "0987654321",
                    DateOfBirth = new DateTime(1992, 3, 20),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            foreach (var customer in customers)
            {
                customer.CustomerCode = $"KH{customer.Id:D6}";
            }

            await context.SaveChangesAsync();
        }

        await context.SaveChangesAsync();
    }
}