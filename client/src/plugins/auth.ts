import { useAuthenticationStore } from '@/stores/authentication'

export default {
  async install() {
    const store = useAuthenticationStore()
    try {
      await store.getUserInfo()
      return
    } catch (error) {
      return
    }
  }
}
