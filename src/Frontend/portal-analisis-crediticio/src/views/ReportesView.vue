<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-blue-50 py-8 px-4">
    <div class="max-w-7xl mx-auto">
      <!-- Encabezado -->
      <div class="flex items-center justify-between mb-6">
        <h1 class="text-2xl font-bold text-gray-800">Reportes</h1>
        <button
          class="flex items-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg shadow transition"
          @click="exportarReportes"
        >
          <ArrowDownTrayIcon class="w-5 h-5" />
          Exportar
        </button>
      </div>

      <!-- Filtros -->
      <div class="bg-white rounded-lg shadow p-4 mb-6">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tipo de Reporte</label>
            <select
              v-model="filtros.tipoReporte"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="">Todos</option>
              <option value="analisis">Análisis Crediticio</option>
              <option value="riesgo">Evaluación de Riesgo</option>
              <option value="tendencias">Tendencias</option>
            </select>
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Fecha Desde</label>
            <input
              type="date"
              v-model="filtros.fechaDesde"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Fecha Hasta</label>
            <input
              type="date"
              v-model="filtros.fechaHasta"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
            />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Estado</label>
            <select
              v-model="filtros.estado"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="">Todos</option>
              <option value="completado">Completado</option>
              <option value="pendiente">Pendiente</option>
              <option value="error">Error</option>
            </select>
          </div>
        </div>
        <div class="flex justify-end mt-4">
          <button
            class="px-4 py-2 bg-gray-200 hover:bg-gray-300 rounded-md mr-2"
            @click="limpiarFiltros"
          >
            Limpiar
          </button>
          <button
            class="px-4 py-2 bg-blue-600 text-white hover:bg-blue-700 rounded-md"
            @click="aplicarFiltros"
          >
            Aplicar Filtros
          </button>
        </div>
      </div>

      <!-- Tabla de Reportes -->
      <div class="bg-white rounded-lg shadow overflow-hidden">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  ID
                </th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Tipo
                </th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Cliente
                </th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Fecha
                </th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Estado
                </th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  Acciones
                </th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="reporte in reportes" :key="reporte.id">
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {{ reporte.id }}
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {{ reporte.tipo }}
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {{ reporte.cliente }}
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                  {{ formatearFecha(reporte.fecha) }}
                </td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span
                    :class="[
                      'px-2 inline-flex text-xs leading-5 font-semibold rounded-full',
                      {
                        'bg-green-100 text-green-800': reporte.estado === 'completado',
                        'bg-yellow-100 text-yellow-800': reporte.estado === 'pendiente',
                        'bg-red-100 text-red-800': reporte.estado === 'error'
                      }
                    ]"
                  >
                    {{ reporte.estado }}
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <div class="flex space-x-3">
                    <button
                      class="text-blue-600 hover:text-blue-900"
                      @click="verReporte(reporte.id)"
                    >
                      Ver
                    </button>
                    <button
                      class="text-green-600 hover:text-green-900"
                      @click="descargarReporte(reporte.id)"
                    >
                      Descargar
                    </button>
                  </div>
                </td>
              </tr>
              <tr v-if="loading">
                <td colspan="6" class="px-6 py-4 text-center">
                  <div class="flex justify-center">
                    <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
                  </div>
                </td>
              </tr>
              <tr v-else-if="reportes.length === 0">
                <td colspan="6" class="px-6 py-4 text-center text-gray-500">
                  No se encontraron reportes
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Paginación -->
        <div class="bg-white px-4 py-3 flex items-center justify-between border-t border-gray-200 sm:px-6">
          <div class="flex-1 flex justify-between sm:hidden">
            <button
              class="relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
              :disabled="paginaActual === 1"
              @click="cambiarPagina(paginaActual - 1)"
            >
              Anterior
            </button>
            <button
              class="ml-3 relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
              :disabled="paginaActual === totalPaginas"
              @click="cambiarPagina(paginaActual + 1)"
            >
              Siguiente
            </button>
          </div>
          <div class="hidden sm:flex-1 sm:flex sm:items-center sm:justify-between">
            <div>
              <p class="text-sm text-gray-700">
                Mostrando
                <span class="font-medium">{{ inicioPagina }}</span>
                a
                <span class="font-medium">{{ finPagina }}</span>
                de
                <span class="font-medium">{{ totalReportes }}</span>
                resultados
              </p>
            </div>
            <div>
              <nav class="relative z-0 inline-flex rounded-md shadow-sm -space-x-px" aria-label="Pagination">
                <button
                  class="relative inline-flex items-center px-2 py-2 rounded-l-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
                  :disabled="paginaActual === 1"
                  @click="cambiarPagina(paginaActual - 1)"
                >
                  <span class="sr-only">Anterior</span>
                  <ChevronLeftIcon class="h-5 w-5" aria-hidden="true" />
                </button>
                <button
                  v-for="pagina in paginas"
                  :key="pagina"
                  :class="[
                    'relative inline-flex items-center px-4 py-2 border text-sm font-medium',
                    pagina === paginaActual
                      ? 'z-10 bg-blue-50 border-blue-500 text-blue-600'
                      : 'bg-white border-gray-300 text-gray-500 hover:bg-gray-50'
                  ]"
                  @click="cambiarPagina(pagina)"
                >
                  {{ pagina }}
                </button>
                <button
                  class="relative inline-flex items-center px-2 py-2 rounded-r-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
                  :disabled="paginaActual === totalPaginas"
                  @click="cambiarPagina(paginaActual + 1)"
                >
                  <span class="sr-only">Siguiente</span>
                  <ChevronRightIcon class="h-5 w-5" aria-hidden="true" />
                </button>
              </nav>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { ArrowDownTrayIcon, ChevronLeftIcon, ChevronRightIcon } from '@heroicons/vue/24/outline'
import { useToast } from 'vue-toastification'
import { reporteService } from '@/services/reporteService'

const toast = useToast()
const loading = ref(false)
const reportes = ref([])
const paginaActual = ref(1)
const totalReportes = ref(0)
const reportesPorPagina = 10

const filtros = ref({
  tipoReporte: '',
  fechaDesde: '',
  fechaHasta: '',
  estado: ''
})

// Computed properties para paginación
const totalPaginas = computed(() => Math.ceil(totalReportes.value / reportesPorPagina))
const inicioPagina = computed(() => (paginaActual.value - 1) * reportesPorPagina + 1)
const finPagina = computed(() => Math.min(paginaActual.value * reportesPorPagina, totalReportes.value))
const paginas = computed(() => {
  const paginas = []
  for (let i = 1; i <= totalPaginas.value; i++) {
    paginas.push(i)
  }
  return paginas
})

// Métodos
function formatearFecha(fecha) {
  return new Date(fecha).toLocaleDateString('es-ES', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

async function cargarReportes() {
  loading.value = true
  try {
    const response = await reporteService.getReportes(
      filtros.value,
      paginaActual.value,
      reportesPorPagina
    )
    reportes.value = response.items
    totalReportes.value = response.total
  } catch (error) {
    toast.error('Error al cargar los reportes')
    console.error('Error:', error)
  } finally {
    loading.value = false
  }
}

function limpiarFiltros() {
  filtros.value = {
    tipoReporte: '',
    fechaDesde: '',
    fechaHasta: '',
    estado: ''
  }
  cargarReportes()
}

function aplicarFiltros() {
  paginaActual.value = 1
  cargarReportes()
}

function cambiarPagina(pagina) {
  paginaActual.value = pagina
  cargarReportes()
}

async function verReporte(id) {
  try {
    const reporte = await reporteService.getReporte(id)
    // Aquí puedes implementar la lógica para mostrar el reporte en un modal o redirigir a una nueva vista
    toast.success('Reporte cargado correctamente')
  } catch (error) {
    toast.error('Error al cargar el reporte')
    console.error('Error:', error)
  }
}

async function descargarReporte(id) {
  try {
    const blob = await reporteService.descargarReporte(id)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `reporte-${id}.pdf`
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    toast.success('Reporte descargado correctamente')
  } catch (error) {
    toast.error('Error al descargar el reporte')
    console.error('Error:', error)
  }
}

async function exportarReportes() {
  try {
    const blob = await reporteService.exportarReportes(filtros.value)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = `reportes-${new Date().toISOString().split('T')[0]}.xlsx`
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    toast.success('Reportes exportados correctamente')
  } catch (error) {
    toast.error('Error al exportar los reportes')
    console.error('Error:', error)
  }
}

onMounted(() => {
  cargarReportes()
})
</script>

<style scoped>
</style> 