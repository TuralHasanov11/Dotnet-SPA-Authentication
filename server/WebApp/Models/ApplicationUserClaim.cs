using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class ApplicationUserClaim : IdentityUserClaim<Guid>
{
    public ApplicationUser User { get; }
}