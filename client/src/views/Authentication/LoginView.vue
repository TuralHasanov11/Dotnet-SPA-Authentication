<script setup lang="ts">
import { ref } from 'vue'
import { useAuthenticationStore } from '@/stores/authentication'
import { Result } from '@/primitives/Result'
import { useRouter } from 'vue-router'

const authStore = useAuthenticationStore()
const router = useRouter()

const email = ref<string>('')
const password = ref<string>('')

async function login(): void {
  const result: Result = await authStore.login(email.value, password.value)

  if (result.isSuccess) {
    return router.replace({ name: 'home' })
  }

  alert(result.error)
}
</script>

<template>
  <section id="login">
    <form @submit.prevent="login">
      <h2>Login</h2>
      <div class="form-group mb-3">
        <label class="form-label" for="email">Email</label>
        <input class="form-control" type="email" id="email" v-model="email" />
      </div>
      <div class="form-group mb-3">
        <label class="form-label" for="password">Password</label>
        <input class="form-control" type="password" id="password" v-model="password" />
      </div>
      <button class="btn btn-primary" type="submit">Login</button>
    </form>
  </section>
</template>
