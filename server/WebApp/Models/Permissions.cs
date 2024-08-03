namespace WebApp.Models;

public static class Permissions
{
    public const string RoleView = "role:view";
    public const string RoleCreate = "role:create";
    public const string RoleUpdate = "role:update";
    public const string RoleDelete = "role:delete";
    public const string UserView = "role:view";
    public const string UserCreate = "role:create";
    public const string UserUpdate = "user:update";
    public const string UserDelete = "user:delete";
    public const string PermissionView = "permission:view";
    public const string PermissionCreate = "permission:create";
    public const string PermissionUpdate = "permission:update";
    public const string PermissionDelete = "permission:delete";
    public const string AccessControlManage = "access-control:manage";

    public static string[] All()
    {
        return
        [
            RoleView,
            RoleCreate,
            RoleUpdate,
            RoleDelete,
            UserView,
            UserCreate,
            UserUpdate,
            UserDelete,
            PermissionView,
            PermissionCreate,
            PermissionUpdate,
            PermissionDelete,
            AccessControlManage
        ];
    }
}