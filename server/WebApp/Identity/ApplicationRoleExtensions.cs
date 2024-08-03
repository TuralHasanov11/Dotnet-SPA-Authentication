using WebApp.Models;
using WebApp.Responses;

namespace WebApp.Identity;

public static class ApplicationRoleExtensions
{
    public static ApplicationRoleResponse ToApplicationRoleResponse(this ApplicationRole role)
    {
        return new ApplicationRoleResponse(role.Id.ToString(), role.Name);
    }
}