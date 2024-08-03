import { watchEffect } from 'vue'
import type { AxiosInstance } from 'axios'
import { privateHttpClient } from '@/utils/httpClient'
import { useAuthenticationStore } from '@/stores/authentication'


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
          !prevRequest.sent &&
          authStore.refreshToken
        ) {
          prevRequest.sent = true
          try {
            const result = await authStore.refresh()
            if (result.isSuccess) {
              prevRequest.headers['Authorization'] = authStore.accessToken
              return privateHttpClient(prevRequest)
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
