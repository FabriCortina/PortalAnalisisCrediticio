import axios from 'axios'

const API_URL = 'http://localhost:5000/api'

export const reporteService = {
  async getReportes(filtros = {}, pagina = 1, itemsPorPagina = 10) {
    try {
      const params = {
        pagina,
        itemsPorPagina,
        ...filtros
      }
      const response = await axios.get(`${API_URL}/reportes`, { params })
      return response.data
    } catch (error) {
      console.error('Error al obtener reportes:', error)
      throw error
    }
  },

  async getReporte(id) {
    try {
      const response = await axios.get(`${API_URL}/reportes/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener reporte:', error)
      throw error
    }
  },

  async descargarReporte(id) {
    try {
      const response = await axios.get(`${API_URL}/reportes/${id}/descargar`, {
        responseType: 'blob'
      })
      return response.data
    } catch (error) {
      console.error('Error al descargar reporte:', error)
      throw error
    }
  },

  async exportarReportes(filtros = {}) {
    try {
      const response = await axios.get(`${API_URL}/reportes/exportar`, {
        params: filtros,
        responseType: 'blob'
      })
      return response.data
    } catch (error) {
      console.error('Error al exportar reportes:', error)
      throw error
    }
  }
} 