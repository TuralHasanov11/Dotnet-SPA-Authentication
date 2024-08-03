using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WebApp.Abstractions;

namespace WebApp.Identity;

public class PermissionClaimsTransformation(IServiceScopeFactory serviceScopeFactory)
    : IClaimsTransformation
{

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {

        if (principal.Identity?.IsAuthenticated == true
            && !principal.HasClaim(c => c.Type == ApplicationClaimTypes.Permission))
        {
            using var scope = serviceScopeFactory.CreateScope();

            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
            var permissions = await permissionService.GetPermissionsForUserAsync(principal.GetUserId());

            var claims = permissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission));
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(claims);
            principal.AddIdentity(claimsIdentity);
        }

        return principal;
    }
}