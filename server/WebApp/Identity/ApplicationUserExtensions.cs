using System.Security.Claims;
using WebApp.Models;
using WebApp.Responses;

namespace WebApp.Identity;

public static class ApplicationUserExtensions
{
    public static HashSet<string> GetPermissions(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal
            .FindAll(ApplicationClaimTypes.Permission)
            .Select(c => c.Value)
            .ToHashSet();
    }

    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {

        var id = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("User id claim not found.");

        return Guid.Parse(id);
    }

    public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.Email)
            ?? throw new InvalidOperationException("User email claim not found.");
    }

    public static UserInfo ToUserInfoResponse(this ApplicationUser user, IEnumerable<string> permissions)
    {
        return new UserInfo(user.Id.ToString(), user.Email, user.UserName, permissions);
    }
}