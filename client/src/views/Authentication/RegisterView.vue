<script setup lang="ts">
import { ref } from 'vue'
import { useAuthenticationStore } from '@/stores/authentication'
import { Result } from '@/primitives/Result'
import { useRouter } from 'vue-router'

const authStore = useAuthenticationStore()
const router = useRouter()

const username = ref<string>('')
const email = ref<string>('')
const password = ref<string>('')
const confirmPassword = ref<string>('')

async function register(): void {
  const result: Result = await authStore.register(
    username.value,
    email.value,
    password.value,
    confirmPassword.value
  )

  if (result.isSuccess) {
    return router.replace({ name: 'authentication-login' })
  }

  alert(result.error)
}
</script>

<template>
  <section id="register">
    <form @submit.prevent="register">
      <h2>Register</h2>
      <div class="form-group mb-3">
        <label class="form-label" for="username">Username</label>
        <input class="form-control" type="text" id="username" v-model="username" />
      </div>
      <div class="form-group mb-3">
        <label class="form-label" for="email">Email</label>
        <input class="form-control" type="email" id="email" v-model="email" />
      </div>
      <div class="form-group mb-3">
        <label class="form-label" for="password">Password</label>
        <input class="form-control" type="password" id="password" v-model="password" />
      </div>
      <div class="form-group mb-3">
        <label class="form-label" for="password">Confirm Password</label>
        <input class="form-control" type="password" id="password" v-model="confirmPassword" />
      </div>
      <button class="btn btn-primary" type="submit">Register</button>
    </form>
  </section>
</template>
