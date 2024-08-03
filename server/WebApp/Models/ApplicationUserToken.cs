using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
    public ApplicationUser User { get; }
}