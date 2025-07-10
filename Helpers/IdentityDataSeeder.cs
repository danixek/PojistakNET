using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PojistakNET.Models;

public static class IdentityDataSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "superadmin", "admin", "user" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static async Task EnsureFirstUserIsSuperAdmin(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var users = userManager.Users.ToList();

        if (users.Count == 1)
        {
            var firstUser = users[0];

            if (!await userManager.IsInRoleAsync(firstUser, "superadmin"))
            {
                await userManager.AddToRoleAsync(firstUser, "superadmin");
            }
        }
    }
}
