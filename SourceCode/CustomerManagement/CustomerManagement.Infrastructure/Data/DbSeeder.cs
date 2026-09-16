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
                    //CustomerCode = "KH000001",
                    FullName = "Nguyễn Văn An",
                    Email = "nguyenvanan@example.com",
                    PhoneNumber = "0901234567",
                    DateOfBirth = new DateTime(1995, 1, 15),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                new()
                {
                    //CustomerCode = "KH000002",
                    FullName = "Trần Thị Bình",
                    Email = "tranthibinh@example.com",
                    PhoneNumber = "0912345678",
                    DateOfBirth = new DateTime(1998, 5, 20),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },

                new()
                {
                    //CustomerCode = "KH000003",
                    FullName = "Lê Minh Cường",
                    Email = "leminhcuong@example.com",
                    PhoneNumber = "0987654321",
                    DateOfBirth = new DateTime(1992, 10, 8),
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Customers.AddRangeAsync(customers);
        }

        await context.SaveChangesAsync();
    }
}