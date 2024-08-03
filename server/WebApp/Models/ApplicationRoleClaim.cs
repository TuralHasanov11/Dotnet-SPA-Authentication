using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    public ApplicationRole Role { get; }
}