export interface User {
  id: string
  email: string
  username: string
  permissions?: string[]
}

export interface Role {
  id: number
  name: string
}

export interface Permission {
  id: number
  name: string
}

export class Permissions {
  public static readonly RoleView: string = 'role:view'
  public static readonly RoleCreate: string = 'role:create'
  public static readonly RoleUpdate: string = 'role:update'
  public static readonly RoleDelete: string = 'role:delete'
  public static readonly UserView: string = 'role:view'
  public static readonly UserCreate: string = 'role:create'
  public static readonly UserUpdate: string = 'user:update'
  public static readonly UserDelete: string = 'user:delete'
  public static readonly PermissionView: string = 'permission:view'
  public static readonly PermissionCreate: string = 'permission:create'
  public static readonly PermissionUpdate: string = 'permission:update'
  public static readonly PermissionDelete: string = 'permission:delete'
  public static readonly AccessControlManage: string = 'access-control:manage'

  public static all(): string[] {
    return [
      this.RoleView,
      this.RoleCreate,
      this.RoleUpdate,
      this.RoleDelete,
      this.UserView,
      this.UserCreate,
      this.UserUpdate,
      this.UserDelete,
      this.PermissionView,
      this.PermissionCreate,
      this.PermissionUpdate,
      this.PermissionDelete,
      this.AccessControlManage
    ]
  }
}
