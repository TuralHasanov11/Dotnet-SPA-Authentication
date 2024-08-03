using Microsoft.AspNetCore.Authorization;

namespace WebApp.Identity;

public class PermissionAuthorizationRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}