<script setup lang="ts">
import { RouterLink, RouterView, useRouter } from 'vue-router'
import { useAuthenticationStore } from '@/stores/authentication'

const authStore = useAuthenticationStore()
const router = useRouter()

async function logout() {
  const result = await authStore.logout()

  if (result.isSuccess) {
    return router.replace({ name: 'home' })
  }
}
</script>

<template>
  <template v-if="authStore.isEmpty">
    <span>Loading...</span>
  </template>
  <template v-else>
    <header>
      <div class="wrapper">
        <nav class="navbar navbar-expand-lg bg-body-tertiary" aria-label="Main navigation">
          <div class="container-fluid">
            <RouterLink class="navbar-brand" :to="{ name: 'home' }">Home</RouterLink>
            <button
              class="navbar-toggler"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#navbarNav"
              aria-controls="navbarNav"
              aria-expanded="false"
              aria-label="Toggle navigation"
            >
              <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
              <ul class="navbar-nav">
                <template v-if="authStore.isAuthenticated">
                  <li class="nav-item">
                    <RouterLink class="nav-link" to="#">{{ authStore?.user?.username }}</RouterLink>
                  </li>

                  <li class="nav-item">
                    <RouterLink
                      activeClass="active"
                      class="nav-link"
                      :to="{ name: 'authentication-permissions' }"
                      >Permissions</RouterLink
                    >
                  </li>

                  <li class="nav-item">
                    <RouterLink
                      activeClass="active"
                      class="nav-link"
                      :to="{ name: 'authorization-roles' }"
                      >Roles</RouterLink
                    >
                  </li>

                  <li class="nav-item">
                    <button @click="logout" class="btn btn-outline-danger">Logout</button>
                  </li>
                </template>
                <template v-else>
                  <li class="nav-item">
                    <RouterLink
                      activeClass="active"
                      class="nav-link"
                      :to="{ name: 'authentication-login' }"
                      >Login</RouterLink
                    >
                  </li>

                  <li class="nav-item">
                    <RouterLink
                      activeClass="active"
                      class="nav-link"
                      :to="{ name: 'authentication-register' }"
                      >Register</RouterLink
                    >
                  </li>
                </template>
              </ul>
            </div>
          </div>
        </nav>
      </div>
    </header>

    <RouterView />
  </template>
</template>
