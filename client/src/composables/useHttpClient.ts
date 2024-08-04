import { watchEffect } from 'vue'
import type { AxiosInstance } from 'axios'
import { privateHttpClient } from '@/utils/httpClient'
import { useAuthenticationStore } from '@/stores/authentication'

let retryCount: number = 0
const MAX_RETRIES = 3

export function useHttpClient(): AxiosInstance {
  const authStore = useAuthenticationStore()

  watchEffect(() => {
    privateHttpClient.interceptors.request.use(
      async (config) => {
        if (!config.headers['Authorization']) {
          config.headers['Authorization'] = `Bearer ${authStore.accessToken}`
        }
        return config
      },
      (error) => {
        console.error('Failed to set token', error)
      }
    )

    privateHttpClient.interceptors.response.use(
      (response) => response,
      async (error) => {
        const prevRequest = error?.config
        if (
          (error?.response?.status === 403 || error?.response?.status === 401) &&
          !prevRequest._retry &&
          authStore.refreshToken.length > 0
        ) {
          if (retryCount >= MAX_RETRIES) {
            window.location.href = '/authentication/login'
            return Promise.reject(error)
          }

          prevRequest._retry = true
          retryCount += 1

          try {
            const result = await authStore.refresh()
            if (result.isSuccess) {
              prevRequest.headers['Authorization'] = `Bearer ${authStore.accessToken}`
              retryCount = 0
              return privateHttpClient(prevRequest)
            } else {
              return Promise.reject(error)
            }
          } catch (error) {
            return Promise.reject(error)
          }
        }

        return Promise.reject(error)
      }
    )
  })

  return privateHttpClient
}
