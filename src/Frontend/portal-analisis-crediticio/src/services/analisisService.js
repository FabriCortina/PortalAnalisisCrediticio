import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

const analisisService = {
  // Obtener métricas generales
  async getMetricas(periodo) {
    try {
      const response = await axios.get(`${API_URL}/analisis/metricas`, {
        params: { periodo }
      })
      return response.data
    } catch (error) {
      console.error('Error al obtener métricas:', error)
      throw error
    }
  },

  // Obtener distribución de riesgos
  async getDistribucionRiesgos(periodo) {
    try {
      const response = await axios.get(`${API_URL}/analisis/distribucion-riesgos`, {
        params: { periodo }
      })
      return response.data
    } catch (error) {
      console.error('Error al obtener distribución de riesgos:', error)
      throw error
    }
  },

  // Obtener tendencias de crédito
  async getTendenciasCredito(periodo) {
    try {
      const response = await axios.get(`${API_URL}/analisis/tendencias`, {
        params: { periodo }
      })
      return response.data
    } catch (error) {
      console.error('Error al obtener tendencias:', error)
      throw error
    }
  },

  // Obtener últimos análisis
  async getUltimosAnalisis() {
    try {
      const response = await axios.get(`${API_URL}/analisis/ultimos`)
      return response.data
    } catch (error) {
      console.error('Error al obtener últimos análisis:', error)
      throw error
    }
  }
}

export default analisisService 