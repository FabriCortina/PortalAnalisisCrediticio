import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL

export const logService = {
  // Obtener logs con filtros y paginación
  async getLogs(filtros = {}, pagina = 1, porPagina = 10, ordenarPor = 'fecha', orden = 'desc') {
    try {
      const response = await axios.get(`${API_URL}/api/logs`, {
        params: {
          ...filtros,
          pagina,
          porPagina,
          ordenarPor,
          orden
        }
      })
      return response.data
    } catch (error) {
      console.error('Error al obtener logs:', error)
      throw error
    }
  },

  // Obtener tipos de acciones disponibles
  async getTiposAccion() {
    try {
      const response = await axios.get(`${API_URL}/api/logs/tipos-accion`)
      return response.data
    } catch (error) {
      console.error('Error al obtener tipos de acción:', error)
      throw error
    }
  },

  // Obtener usuarios que han realizado acciones
  async getUsuarios() {
    try {
      const response = await axios.get(`${API_URL}/api/logs/usuarios`)
      return response.data
    } catch (error) {
      console.error('Error al obtener usuarios:', error)
      throw error
    }
  }
} 