import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import { useAuthenticationStore } from '@/stores/authentication'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/authentication',
      name: 'authentication',
      children: [
        {
          path: 'login',
          name: 'authentication-login',
          component: () => import('../views/Authentication/LoginView.vue')
        },
        {
          path: 'register',
          name: 'authentication-register',
          component: () => import('../views/Authentication/RegisterView.vue')
        },
        {
          path: 'permissions',
          name: 'authentication-permissions',
          component: () => import('../views/Authentication/PermissionsView.vue'),
          meta: { requiresAuth: true }
        }
      ]
    },
    {
      path: '/authorization',
      name: 'authorization',
      children: [
        {
          path: 'roles',
          name: 'authorization-roles',
          component: () => import('../views/Authorization/RolesView.vue'),
          meta: { requiresPermission: 'role:view' }
        }
      ]
    }
  ]
})

router.beforeResolve(async (to, from, next) => {
  const authStore = useAuthenticationStore()

  if (authStore.isEmpty) {
    await authStore.getUserInfo()
    console.log('User info fetched')
  }

  if (
    to.meta.requiresPermission &&
    !authStore.hasPermission(to.meta.requiresPermission as string)
  ) {
    return next({ name: 'home' })
  } else if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return next({ name: 'authentication-login', query: { redirect: to.fullPath } })
  } else if (to.meta.requiresGuest && authStore.isAuthenticated) {
    return next({ name: 'home' })
  } else {
    return next()
  }
})

export default router
