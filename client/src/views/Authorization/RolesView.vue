<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useAuthorizationStore } from '@/stores/authorization'
import { useAuthenticationStore } from '@/stores/authentication'
import { Permissions } from '@/models'

const authorizationStore = useAuthorizationStore()
const authenticationStore = useAuthenticationStore()
const name = ref<string>('')

async function createRole(): void {
  const result = await authorizationStore.createRole(name.value)

  if (result.isSuccess) {
    name.value = ''
    await authorizationStore.getRoles()
  }
}

onMounted(async () => {
  await authorizationStore.getRoles()
})
</script>
<template>
  <section id="roles">
    <h1>Roles</h1>

    <section id="createRole" class="py-3">
      <template v-if="authenticationStore.hasPermission(Permissions.RoleCreate)">
        <h5>Create Role</h5>
        <form @submit.prevent="createRole">
          <div class="form-group mb-3">
            <label class="form-label" for="name">Name</label>
            <input class="form-control" type="text" id="name" v-model="name" />
          </div>
          <button class="btn btn-success" type="submit">Create</button>
        </form>
      </template>
    </section>

    <hr />

    <ul>
      <li v-for="role in authorizationStore.roles" :key="role.id">
        <span>{{ role.name }}</span>
      </li>
    </ul>
  </section>
</template>
