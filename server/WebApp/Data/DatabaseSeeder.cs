using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.Roles.AnyAsync())
        {
            foreach (var role in ApplicationRoles.All())
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role
                });
            }

            await dbContext.Permissions.AddRangeAsync(Permissions.All().Select(p => Permission.Create(p)));

            await dbContext.SaveChangesAsync();

            var adminRole = await roleManager.FindByNameAsync(ApplicationRoles.Administrator);

            if (adminRole is null)
            {
                return;
            }

            var permissions = await dbContext.Permissions.ToListAsync();

            adminRole.AddPermissions(permissions);
            await roleManager.UpdateAsync(adminRole);

            var adminUser = ApplicationUser.Create();
            await userManager.SetEmailAsync(adminUser, configuration["Admin:Email"]);
            await userManager.SetUserNameAsync(adminUser, configuration["Admin:Username"]);
            await userManager.CreateAsync(adminUser, configuration["Admin:Password"]!);
            await userManager.AddToRoleAsync(adminUser, ApplicationRoles.Administrator);
        }
    }
}