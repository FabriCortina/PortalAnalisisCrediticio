import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authService } from '@/services/authService'
import { useToast } from 'vue-toastification'

export const useAuthStore = defineStore('auth', () => {
  const toast = useToast()
  const user = ref(null)
  const loading = ref(false)
  const error = ref(null)

  // Getters
  const isAuthenticated = computed(() => !!user.value)
  const userRole = computed(() => user.value?.role)
  const userName = computed(() => user.value?.name)

  // Acciones
  async function login(email, password) {
    loading.value = true
    error.value = null
    try {
      const userData = await authService.login(email, password)
      user.value = userData
      toast.success('Inicio de sesión exitoso')
      return userData
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al iniciar sesión'
      toast.error(error.value)
      throw err
    } finally {
      loading.value = false
    }
  }

  async function logout() {
    loading.value = true
    try {
      await authService.logout()
      user.value = null
      toast.success('Sesión cerrada correctamente')
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al cerrar sesión'
      toast.error(error.value)
    } finally {
      loading.value = false
    }
  }

  async function fetchUser() {
    loading.value = true
    try {
      const userData = await authService.getCurrentUser()
      user.value = userData
      return userData
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al obtener datos del usuario'
      toast.error(error.value)
      throw err
    } finally {
      loading.value = false
    }
  }

  async function refreshToken() {
    try {
      const token = await authService.refreshToken()
      return token
    } catch (err) {
      error.value = err.response?.data?.message || 'Error al refrescar el token'
      toast.error(error.value)
      throw err
    }
  }

  // Inicializar el store
  function initialize() {
    const storedUser = authService.getUser()
    if (storedUser) {
      user.value = storedUser
    }
  }

  return {
    // Estado
    user,
    loading,
    error,

    // Getters
    isAuthenticated,
    userRole,
    userName,

    // Acciones
    login,
    logout,
    fetchUser,
    refreshToken,
    initialize
  }
}) 