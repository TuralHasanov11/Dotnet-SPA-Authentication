import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Role } from '@/models'
import { Result } from '@/primitives/Result'
import type { CreateRoleRequest } from '@/requests'
import { useHttpClient } from '@/composables/useHttpClient'

export const useAuthorizationStore = defineStore('authorization', () => {
  const roles = ref<Role[]>([])

  async function getRoles(): Promise<void> {
    try {
      const { data } = await useHttpClient().get<Role[]>('api/authorization/roles')

      roles.value = data
    } catch (error) {
      console.log(error)
    }
  }

  async function createRole(name: string): Promise<Result<string>> {
    try {
      await useHttpClient().post<CreateRoleRequest>('api/authorization/roles', {
        name: name
      })
    } catch (error) {
      console.log(error)

      // return Result.failure('Failed to create role')
    }

    return Result.success('Registration successful')
  }

  return { roles, createRole, getRoles }
})
