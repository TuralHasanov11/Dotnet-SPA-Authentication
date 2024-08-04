import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import type { User } from '@/models'
import { Result } from '@/primitives/Result'
import type { LoginRequest, RefreshTokenRequest, RegisterRequest } from '@/requests'
import type { AccessTokenResponse, UserResponse } from '@/responses'
import Mapper from '@/mappers'
import { useHttpClient } from '@/composables/useHttpClient'
import { AppError } from '@/primitives/Error'
import type { AxiosError, AxiosResponse } from 'axios'

const nullUser: User = {
  id: '',
  email: '',
  username: '',
  permissions: []
}

export const useAuthenticationStore = defineStore('authentication', () => {
  const user = ref<User>()
  const isAuthenticated = computed(() => !isLoading.value && user?.value?.id !== nullUser.id)
  const isLoading = computed(() => user?.value === undefined)
  const accessToken = ref<string>('')
  const refreshToken = ref<string>('')

  function hasPermission(permission: string): boolean {
    return (isAuthenticated.value && user.value?.permissions?.includes(permission)) ?? false
  }

  async function register(
    username: string,
    email: string,
    password: string,
    confirmPassword: string
  ): Promise<Result<string>> {
    try {
      await useHttpClient().post<RegisterRequest>('api/authentication/register', {
        username: username,
        email: email,
        password: password,
        confirmPassword: confirmPassword
      })
    } catch (error) {
      console.log(error)
    }

    return Result.success('Registration successful')
  }

  async function login(email: string, password: string): Promise<Result<string>> {
    try {
      const { data } = await useHttpClient().post<LoginRequest, AxiosResponse<AccessTokenResponse>>(
        'api/authentication/login',
        {
          email: email,
          password: password
        }
      )

      accessToken.value = data.accessToken
      refreshToken.value = data.refreshToken

      await getUserInfo()
    } catch (error) {
      const apiError = error as AxiosError
      return Result.failure(AppError.failure(apiError.message))
    }

    return Result.success('Login successful')
  }

  async function logout(): Promise<Result<string>> {
    try {
      await useHttpClient().get('api/authentication/logout')
    } catch (error) {
      const apiError = error as AxiosError
    }

    user.value = nullUser
    accessToken.value = ''
    refreshToken.value = ''

    return Result.success('Logout successful')
  }

  async function getUserInfo(): Promise<Result<string>> {
    try {
      const { data } = await useHttpClient().get<UserResponse>('api/authentication/user-info')

      user.value = Mapper.toUser(data)
    } catch (error: any) {
      user.value = nullUser
      const apiError = error as AxiosError
      return Result.failure(AppError.failure(apiError.message))
    }

    return Result.success('Logout successful')
  }

  async function refresh(): Promise<Result<string>> {
    try {
      const { data } = await useHttpClient().post<
        RefreshTokenRequest,
        AxiosResponse<AccessTokenResponse>
      >('api/authentication/refresh', {
        refreshToken: refreshToken.value,
        accessToken: accessToken.value
      })

      accessToken.value = data.accessToken
      refreshToken.value = data.refreshToken
      return Result.success(data.accessToken)
    } catch (error) {
      const apiError = error as AxiosError
      return Result.failure(AppError.failure(apiError.message))
    }
  }

  return {
    user,
    isAuthenticated,
    register,
    login,
    logout,
    getUserInfo,
    hasPermission,
    isLoading,
    accessToken,
    refreshToken,
    refresh
  }
})
