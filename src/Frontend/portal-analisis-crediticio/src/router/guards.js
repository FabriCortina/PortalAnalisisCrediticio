import { useAuthStore } from '@/stores/authStore'

export const authGuard = async (to, from, next) => {
  const authStore = useAuthStore()
  
  // Si la ruta requiere autenticación
  if (to.meta.requiresAuth) {
    // Si no está autenticado
    if (!authStore.isAuthenticated) {
      try {
        // Intentar obtener el usuario actual
        await authStore.fetchUser()
        next()
      } catch (error) {
        // Si falla, redirigir al login
        next({
          path: '/login',
          query: { redirect: to.fullPath }
        })
      }
    } else {
      // Si está autenticado, permitir el acceso
      next()
    }
  } else {
    // Si la ruta no requiere autenticación
    next()
  }
}

export const guestGuard = (to, from, next) => {
  const authStore = useAuthStore()
  
  // Si está autenticado y trata de acceder a rutas de invitado (login, registro)
  if (authStore.isAuthenticated) {
    next({ path: '/' })
  } else {
    next()
  }
} 