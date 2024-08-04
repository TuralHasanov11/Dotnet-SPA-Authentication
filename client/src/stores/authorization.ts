import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Role } from '@/models'
import { Result } from '@/primitives/Result'
import type { CreateRoleRequest } from '@/requests'
import { usePrivateHttpClient } from '@/composables/useHttpClient'
import { AppError } from '@/primitives/Error'
import type { AxiosError } from 'axios'

export const useAuthorizationStore = defineStore('authorization', () => {
  const roles = ref<Role[]>([])

  async function getRoles(): Promise<void> {
    try {
      const { data } = await usePrivateHttpClient().get<Role[]>('api/authorization/roles')

      roles.value = data
    } catch (error) {
      console.log(error)
    }
  }

  async function createRole(name: string): Promise<Result<string>> {
    try {
      await usePrivateHttpClient().post<CreateRoleRequest>('api/authorization/roles', {
        name: name
      })
    } catch (error) {
      const apiError = error as AxiosError
      return Result.failure(AppError.failure(apiError.message))
    }

    return Result.success('Registration successful')
  }

  return { roles, createRole, getRoles }
})
