using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class ApplicationUserLogin : IdentityUserLogin<Guid>
{
    public ApplicationUser User { get; }
}