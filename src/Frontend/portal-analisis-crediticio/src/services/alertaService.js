import axios from 'axios'

const API_URL = 'http://localhost:5000/api'

export const alertaService = {
  async getAlertas(filtros = {}, pagina = 1, itemsPorPagina = 10) {
    try {
      const params = {
        pagina,
        itemsPorPagina,
        ...filtros
      }
      const response = await axios.get(`${API_URL}/alertas`, { params })
      return response.data
    } catch (error) {
      console.error('Error al obtener alertas:', error)
      throw error
    }
  },

  async getAlerta(id) {
    try {
      const response = await axios.get(`${API_URL}/alertas/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener alerta:', error)
      throw error
    }
  },

  async marcarComoLeida(id) {
    try {
      const response = await axios.put(`${API_URL}/alertas/${id}/leer`)
      return response.data
    } catch (error) {
      console.error('Error al marcar alerta como leída:', error)
      throw error
    }
  },

  async marcarTodasComoLeidas() {
    try {
      const response = await axios.put(`${API_URL}/alertas/leer-todas`)
      return response.data
    } catch (error) {
      console.error('Error al marcar todas las alertas como leídas:', error)
      throw error
    }
  },

  async eliminarAlerta(id) {
    try {
      const response = await axios.delete(`${API_URL}/alertas/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al eliminar alerta:', error)
      throw error
    }
  }
} 