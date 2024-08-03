using Microsoft.AspNetCore.Authorization;

namespace WebApp.Identity;

public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == true
            && context.User.GetPermissions().Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}