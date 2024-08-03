using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<ApplicationUserRole> UserRoles { get; } = [];

    public ICollection<ApplicationRoleClaim> RoleClaims { get; } = [];

    public ICollection<Permission> Permissions { get; } = [];

    public void AddPermission(Permission permission)
    {
        Permissions.Add(permission);
    }

    public void RemovePermission(Permission permission)
    {
        Permissions.Remove(permission);
    }

    public void AddPermissions(IEnumerable<Permission> permissions)
    {
        foreach (var permission in permissions)
        {
            AddPermission(permission);
        }
    }
}