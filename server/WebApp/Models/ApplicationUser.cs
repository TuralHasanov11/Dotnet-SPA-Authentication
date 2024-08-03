using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<ApplicationUserClaim> Claims { get; } = [];
    public ICollection<ApplicationUserLogin> Logins { get; } = [];
    public ICollection<ApplicationUserToken> Tokens { get; } = [];
    public ICollection<ApplicationUserRole> UserRoles { get; } = [];

    private ApplicationUser() { }

    public static ApplicationUser Create()
    {
        return new ApplicationUser();
    }
}