import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL

export const creditoService = {
  // Obtener todos los créditos activos
  async getCreditosActivos(filtros = {}) {
    try {
      const response = await axios.get(`${API_URL}/api/creditos/activos`, {
        params: filtros
      })
      return response.data
    } catch (error) {
      console.error('Error al obtener créditos activos:', error)
      throw error
    }
  },

  // Obtener detalles de un crédito específico
  async getCreditoDetalle(id) {
    try {
      const response = await axios.get(`${API_URL}/api/creditos/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener detalle del crédito:', error)
      throw error
    }
  },

  // Registrar pago de cuota
  async registrarPagoCuota(creditoId, cuotaId, datosPago) {
    try {
      const response = await axios.post(
        `${API_URL}/api/creditos/${creditoId}/cuotas/${cuotaId}/pago`,
        datosPago
      )
      return response.data
    } catch (error) {
      console.error('Error al registrar pago:', error)
      throw error
    }
  },

  // Obtener créditos próximos a vencer
  async getCreditosProximosAVencer() {
    try {
      const response = await axios.get(`${API_URL}/api/creditos/proximos-vencer`)
      return response.data
    } catch (error) {
      console.error('Error al obtener créditos próximos a vencer:', error)
      throw error
    }
  }
} 