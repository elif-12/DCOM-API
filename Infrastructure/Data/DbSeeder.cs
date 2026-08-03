using DCOM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DCOM_API.Data;

public static class DbSeeder
{
    public static async Task SeedSuperAdminAsync(AppDbContext context, IConfiguration config)
    {
        // En az bir süper admin varsa hiçbir şey yapma
        if (await context.Users.AnyAsync(u => u.Role == UserRole.SuperAdmin))
            return;

        var section = config.GetSection("SuperAdmin");
        var username = section["Username"] ?? "admin";
        var password = section["Password"] ?? "Admin123!";
        var fullName = section["FullName"] ?? "Süper Admin";

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            FullName = fullName,
            Role = UserRole.SuperAdmin,
            IsActive = true,
            
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
