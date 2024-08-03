using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; }
    public ApplicationRole Role { get; }
}