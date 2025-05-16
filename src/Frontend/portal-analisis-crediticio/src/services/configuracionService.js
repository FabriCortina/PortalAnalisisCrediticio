import axios from 'axios'

const API_URL = 'http://localhost:5000/api'

export const configuracionService = {
  async getConfiguracion() {
    try {
      const response = await axios.get(`${API_URL}/configuracion`)
      return response.data
    } catch (error) {
      console.error('Error al obtener la configuración:', error)
      throw error
    }
  },

  async guardarConfiguracion(configuracion) {
    try {
      const response = await axios.put(`${API_URL}/configuracion`, configuracion)
      return response.data
    } catch (error) {
      console.error('Error al guardar la configuración:', error)
      throw error
    }
  },

  async cambiarContraseña(datos) {
    try {
      const response = await axios.put(`${API_URL}/configuracion/cambiar-contraseña`, datos)
      return response.data
    } catch (error) {
      console.error('Error al cambiar la contraseña:', error)
      throw error
    }
  },

  async actualizarNotificaciones(notificaciones) {
    try {
      const response = await axios.put(`${API_URL}/configuracion/notificaciones`, notificaciones)
      return response.data
    } catch (error) {
      console.error('Error al actualizar las notificaciones:', error)
      throw error
    }
  },

  async actualizarPersonalizacion(personalizacion) {
    try {
      const response = await axios.put(`${API_URL}/configuracion/personalizacion`, personalizacion)
      return response.data
    } catch (error) {
      console.error('Error al actualizar la personalización:', error)
      throw error
    }
  }
} 