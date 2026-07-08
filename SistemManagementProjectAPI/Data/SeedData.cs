using Microsoft.AspNetCore.Identity;
using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.Data;

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        
        var adminEmail = "admin@sistemmanagementproject.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            
            var result = await userManager.CreateAsync(adminUser, "Rahasia123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"Sample user created successfully with email: {adminEmail}");
                Console.WriteLine("Default password: Rahasia123");
            }
        }
    }
}