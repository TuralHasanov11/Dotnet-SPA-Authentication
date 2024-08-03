import type { User } from '@/models'
import type { UserResponse } from '@/responses'

export default class Mapper {
  public static toUser(value: UserResponse): User {
    return {
      id: value.id,
      email: value.email,
      username: value.username,
      permissions: value.permissions
    }
  }
}
