import axios from 'axios'
import { cacheService } from './cacheService'

const API_URL = import.meta.env.VITE_API_URL

export const dashboardService = {
  // Obtener KPIs del dashboard
  async getKPIs() {
    return cacheService.getOrSet('dashboard_kpis', async () => {
      try {
        const response = await axios.get(`${API_URL}/api/dashboard/kpis`)
        return response.data
      } catch (error) {
        console.error('Error al obtener KPIs:', error)
        throw error
      }
    }, 5 * 60 * 1000) // 5 minutos
  },

  // Obtener distribución de riesgos
  async getDistribucionRiesgos() {
    return cacheService.getOrSet('dashboard_riesgos', async () => {
      try {
        const response = await axios.get(`${API_URL}/api/dashboard/distribucion-riesgos`)
        return response.data
      } catch (error) {
        console.error('Error al obtener distribución de riesgos:', error)
        throw error
      }
    }, 5 * 60 * 1000) // 5 minutos
  },

  // Obtener solicitudes por mes
  async getSolicitudesPorMes() {
    return cacheService.getOrSet('dashboard_solicitudes', async () => {
      try {
        const response = await axios.get(`${API_URL}/api/dashboard/solicitudes-mes`)
        return response.data
      } catch (error) {
        console.error('Error al obtener solicitudes por mes:', error)
        throw error
      }
    }, 5 * 60 * 1000) // 5 minutos
  },

  // Limpiar caché del dashboard
  clearCache() {
    cacheService.delete('dashboard_kpis')
    cacheService.delete('dashboard_riesgos')
    cacheService.delete('dashboard_solicitudes')
  }
} 