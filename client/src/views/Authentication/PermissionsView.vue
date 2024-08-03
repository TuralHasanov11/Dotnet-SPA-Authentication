<script setup lang="ts">
import { useAuthenticationStore } from '@/stores/authentication'

const authStore = useAuthenticationStore()
</script>

<template>
  <section id="permissions">
    <h1>Permissions</h1>
    <ul>
      <li v-for="permission in authStore.user.permissions" :key="permission">
        <span>{{ permission }}</span>
      </li>
    </ul>
  </section>
</template>
