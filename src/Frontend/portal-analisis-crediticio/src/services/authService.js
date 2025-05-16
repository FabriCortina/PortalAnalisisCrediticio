import axios from 'axios'

const API_URL = 'http://localhost:5000/api'

// Configurar axios para incluir el token en todas las peticiones
axios.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Interceptor para manejar errores de autenticación
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export const authService = {
  async login(email, password) {
    try {
      const response = await axios.post(`${API_URL}/auth/login`, {
        email,
        password
      })
      
      const { token, user } = response.data
      
      // Guardar token y datos del usuario
      localStorage.setItem('token', token)
      localStorage.setItem('user', JSON.stringify(user))
      
      return user
    } catch (error) {
      console.error('Error en login:', error)
      throw error
    }
  },

  async logout() {
    try {
      await axios.post(`${API_URL}/auth/logout`)
    } catch (error) {
      console.error('Error en logout:', error)
    } finally {
      // Limpiar datos de sesión
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    }
  },

  async getCurrentUser() {
    try {
      const response = await axios.get(`${API_URL}/auth/me`)
      const user = response.data
      localStorage.setItem('user', JSON.stringify(user))
      return user
    } catch (error) {
      console.error('Error al obtener usuario actual:', error)
      throw error
    }
  },

  async refreshToken() {
    try {
      const response = await axios.post(`${API_URL}/auth/refresh-token`)
      const { token } = response.data
      localStorage.setItem('token', token)
      return token
    } catch (error) {
      console.error('Error al refrescar token:', error)
      throw error
    }
  },

  isAuthenticated() {
    return !!localStorage.getItem('token')
  },

  getToken() {
    return localStorage.getItem('token')
  },

  getUser() {
    const user = localStorage.getItem('user')
    return user ? JSON.parse(user) : null
  }
} 