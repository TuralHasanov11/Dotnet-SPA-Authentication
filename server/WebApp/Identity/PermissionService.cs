using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Abstractions;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Identity;

public class PermissionService(
    ApplicationDbContext dbContext,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager) : IPermissionService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IEnumerable<Permission>> GetPermissionsAsync()
    {
        return await _dbContext.Permissions.ToListAsync();
    }

    public async Task<IEnumerable<string>> GetPermissionsForUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return [];
        }

        var roleNames = await _userManager.GetRolesAsync(user);

        return await _roleManager.Roles
            .Include(r => r.Permissions)
            .Where(r => roleNames.Contains(r.Name!))
            .SelectMany(r => r.Permissions)
            .Select(p => p.Name)
            .Distinct()
            .ToArrayAsync();
    }
}