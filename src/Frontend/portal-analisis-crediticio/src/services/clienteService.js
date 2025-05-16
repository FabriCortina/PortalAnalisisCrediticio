import api from '@/config/api'

export default {
  async getClientes(filtros = {}, pagina = 1, itemsPorPagina = 10) {
    try {
      const response = await api.get('/clientes', {
        params: {
          ...filtros,
          pagina,
          itemsPorPagina
        }
      })
      return {
        data: response.data,
        total: response.headers['x-total-count'] || response.data.length
      }
    } catch (error) {
      console.error('Error al obtener clientes:', error)
      throw error
    }
  },

  async getCliente(id) {
    try {
      const response = await api.get(`/clientes/${id}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener cliente:', error)
      throw error
    }
  },

  async createCliente(cliente) {
    try {
      const response = await api.post('/clientes', cliente)
      return response.data
    } catch (error) {
      console.error('Error al crear cliente:', error)
      throw error
    }
  },

  async updateCliente(id, cliente) {
    try {
      const response = await api.put(`/clientes/${id}`, cliente)
      return response.data
    } catch (error) {
      console.error('Error al actualizar cliente:', error)
      throw error
    }
  },

  async deleteCliente(id) {
    try {
      await api.delete(`/clientes/${id}`)
      return true
    } catch (error) {
      console.error('Error al eliminar cliente:', error)
      throw error
    }
  },

  async getInformacionFinanciera(clienteId) {
    try {
      const response = await api.get(`/clientes/${clienteId}/informacion-financiera`)
      return response.data
    } catch (error) {
      console.error('Error al obtener información financiera:', error)
      throw error
    }
  },

  async actualizarInformacionFinanciera(clienteId, { tipo, datos }) {
    try {
      const response = await api.put(`/clientes/${clienteId}/informacion-financiera/${tipo}`, datos)
      return response.data
    } catch (error) {
      console.error('Error al actualizar información financiera:', error)
      throw error
    }
  },

  async importarExcel(clienteId, archivo) {
    try {
      const formData = new FormData()
      formData.append('archivo', archivo)

      const response = await api.post(`/clientes/${clienteId}/importar-excel`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
      return response.data
    } catch (error) {
      console.error('Error al importar Excel:', error)
      throw error
    }
  },

  async exportarExcel(clienteId) {
    try {
      const response = await api.get(`/clientes/${clienteId}/exportar-excel`, {
        responseType: 'blob'
      })
      return response.data
    } catch (error) {
      console.error('Error al exportar Excel:', error)
      throw error
    }
  },

  async getSolicitudesProducto(clienteId) {
    try {
      const response = await api.get(`/solicitudesproducto/cliente/${clienteId}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener solicitudes de producto:', error)
      throw error
    }
  },

  async getInformesCrediticio(clienteId) {
    try {
      const response = await api.get(`/informescrediticio/cliente/${clienteId}`)
      return response.data
    } catch (error) {
      console.error('Error al obtener informes crediticios:', error)
      throw error
    }
  },

  async getArchivos(clienteId, params = {}) {
    try {
      const response = await api.get(`/api/clientes/${clienteId}/archivos`, { params })
      return response.data
    } catch (error) {
      console.error('Error al obtener archivos:', error)
      throw error
    }
  },

  async subirArchivo(clienteId, formData) {
    try {
      const response = await api.post(`/api/clientes/${clienteId}/archivos`, formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
      return response.data
    } catch (error) {
      console.error('Error al subir archivo:', error)
      throw error
    }
  },

  async eliminarArchivo(clienteId, archivoId) {
    try {
      await api.delete(`/api/clientes/${clienteId}/archivos/${archivoId}`)
      return true
    } catch (error) {
      console.error('Error al eliminar archivo:', error)
      throw error
    }
  },

  async obtenerUrlPrevisualizacion(archivoId) {
    try {
      const response = await api.get(`/api/archivos/${archivoId}/preview`)
      return response.data.url
    } catch (error) {
      console.error('Error al obtener URL de previsualización:', error)
      throw error
    }
  },

  async obtenerUrlDescarga(archivoId) {
    try {
      const response = await api.get(`/api/archivos/${archivoId}/download`)
      return response.data.url
    } catch (error) {
      console.error('Error al obtener URL de descarga:', error)
      throw error
    }
  },

  async getNotas(archivoId) {
    try {
      const response = await api.get(`/api/archivos/${archivoId}/notas`)
      return response.data
    } catch (error) {
      console.error('Error al obtener notas:', error)
      throw error
    }
  },

  async guardarNotas(archivoId, notas) {
    try {
      const response = await api.post(`/api/archivos/${archivoId}/notas`, notas)
      return response.data
    } catch (error) {
      console.error('Error al guardar notas:', error)
      throw error
    }
  },

  async getHistorialNotas(archivoId) {
    try {
      const response = await api.get(`/api/archivos/${archivoId}/notas/historial`)
      return response.data
    } catch (error) {
      console.error('Error al obtener historial de notas:', error)
      throw error
    }
  }
} 