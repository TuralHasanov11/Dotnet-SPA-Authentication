using WebApp.Models;

namespace WebApp.Abstractions;

public interface IPermissionService
{
    Task<IEnumerable<Permission>> GetPermissionsAsync();

    Task<IEnumerable<string>> GetPermissionsForUserAsync(Guid userId);
}