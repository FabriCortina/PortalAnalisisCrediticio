<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-blue-50 py-8 px-4">
    <div class="max-w-7xl mx-auto">
      <!-- Encabezado -->
      <div class="flex items-center justify-between mb-6">
        <h1 class="text-2xl font-bold text-gray-800">Alertas</h1>
        <button
          class="flex items-center gap-2 bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg shadow transition"
          @click="marcarTodasComoLeidas"
          :disabled="loading"
        >
          <CheckCircleIcon class="w-5 h-5" />
          Marcar todas como leídas
        </button>
      </div>

      <!-- Filtros -->
      <div class="bg-white rounded-lg shadow p-4 mb-6">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Tipo de Alerta</label>
            <select
              v-model="filtros.tipo"
              class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="">Todos</option>
              <option value="riesgo">Riesgo Crediticio</option>
              <option value="vencimiento">Vencimiento</option>
              <option value="cambio">Cambio de Estado</option>
              <option value="sistema">Sistema</option>
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
              <option value="no_leida">No Leída</option>
              <option value="leida">Leída</option>
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

      <!-- Lista de Alertas -->
      <div class="bg-white rounded-lg shadow overflow-hidden">
        <div class="divide-y divide-gray-200">
          <div
            v-for="alerta in alertas"
            :key="alerta.id"
            class="p-4 hover:bg-gray-50 transition-colors"
            :class="{ 'bg-blue-50': !alerta.leida }"
          >
            <div class="flex items-start justify-between">
              <div class="flex items-start space-x-3">
                <div
                  class="flex-shrink-0 w-2 h-2 mt-2 rounded-full"
                  :class="{
                    'bg-red-500': alerta.tipo === 'riesgo',
                    'bg-yellow-500': alerta.tipo === 'vencimiento',
                    'bg-blue-500': alerta.tipo === 'cambio',
                    'bg-gray-500': alerta.tipo === 'sistema'
                  }"
                ></div>
                <div>
                  <div class="flex items-center space-x-2">
                    <h3 class="text-sm font-medium text-gray-900">{{ alerta.titulo }}</h3>
                    <span
                      v-if="!alerta.leida"
                      class="px-2 py-1 text-xs font-medium text-blue-800 bg-blue-100 rounded-full"
                    >
                      Nueva
                    </span>
                  </div>
                  <p class="mt-1 text-sm text-gray-500">{{ alerta.descripcion }}</p>
                  <div class="mt-2 flex items-center text-xs text-gray-500">
                    <ClockIcon class="w-4 h-4 mr-1" />
                    {{ formatearFecha(alerta.fecha) }}
                  </div>
                </div>
              </div>
              <div class="flex items-center space-x-2">
                <button
                  v-if="!alerta.leida"
                  class="text-blue-600 hover:text-blue-800"
                  @click="marcarComoLeida(alerta.id)"
                >
                  <CheckCircleIcon class="w-5 h-5" />
                </button>
                <button
                  class="text-red-600 hover:text-red-800"
                  @click="confirmarEliminar(alerta.id)"
                >
                  <TrashIcon class="w-5 h-5" />
                </button>
              </div>
            </div>
          </div>
          <div v-if="loading" class="p-8 text-center">
            <div class="flex justify-center">
              <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
            </div>
          </div>
          <div v-else-if="alertas.length === 0" class="p-8 text-center text-gray-500">
            No se encontraron alertas
          </div>
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
                <span class="font-medium">{{ totalAlertas }}</span>
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

    <!-- Modal de confirmación de eliminación -->
    <div v-if="showDeleteModal" class="fixed inset-0 bg-black bg-opacity-30 flex items-center justify-center z-50">
      <div class="bg-white rounded-lg shadow-lg p-8 w-full max-w-md">
        <h2 class="text-lg font-semibold mb-4">Confirmar Eliminación</h2>
        <p class="text-gray-600 mb-6">¿Está seguro que desea eliminar esta alerta?</p>
        <div class="flex justify-end gap-2">
          <button
            class="px-4 py-2 rounded bg-gray-200 hover:bg-gray-300"
            @click="showDeleteModal = false"
          >
            Cancelar
          </button>
          <button
            class="px-4 py-2 rounded bg-red-600 text-white hover:bg-red-700"
            @click="eliminarAlerta"
            :disabled="deleting"
          >
            {{ deleting ? 'Eliminando...' : 'Eliminar' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import {
  CheckCircleIcon,
  ClockIcon,
  TrashIcon,
  ChevronLeftIcon,
  ChevronRightIcon
} from '@heroicons/vue/24/outline'
import { useToast } from 'vue-toastification'
import { alertaService } from '@/services/alertaService'

const toast = useToast()
const loading = ref(false)
const deleting = ref(false)
const alertas = ref([])
const paginaActual = ref(1)
const totalAlertas = ref(0)
const alertasPorPagina = 10
const showDeleteModal = ref(false)
const alertaToDelete = ref(null)

const filtros = ref({
  tipo: '',
  fechaDesde: '',
  fechaHasta: '',
  estado: ''
})

// Computed properties para paginación
const totalPaginas = computed(() => Math.ceil(totalAlertas.value / alertasPorPagina))
const inicioPagina = computed(() => (paginaActual.value - 1) * alertasPorPagina + 1)
const finPagina = computed(() => Math.min(paginaActual.value * alertasPorPagina, totalAlertas.value))
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
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

async function cargarAlertas() {
  loading.value = true
  try {
    const response = await alertaService.getAlertas(
      filtros.value,
      paginaActual.value,
      alertasPorPagina
    )
    alertas.value = response.items
    totalAlertas.value = response.total
  } catch (error) {
    toast.error('Error al cargar las alertas')
    console.error('Error:', error)
  } finally {
    loading.value = false
  }
}

function limpiarFiltros() {
  filtros.value = {
    tipo: '',
    fechaDesde: '',
    fechaHasta: '',
    estado: ''
  }
  cargarAlertas()
}

function aplicarFiltros() {
  paginaActual.value = 1
  cargarAlertas()
}

function cambiarPagina(pagina) {
  paginaActual.value = pagina
  cargarAlertas()
}

async function marcarComoLeida(id) {
  try {
    await alertaService.marcarComoLeida(id)
    toast.success('Alerta marcada como leída')
    await cargarAlertas()
  } catch (error) {
    toast.error('Error al marcar la alerta como leída')
    console.error('Error:', error)
  }
}

async function marcarTodasComoLeidas() {
  try {
    await alertaService.marcarTodasComoLeidas()
    toast.success('Todas las alertas han sido marcadas como leídas')
    await cargarAlertas()
  } catch (error) {
    toast.error('Error al marcar las alertas como leídas')
    console.error('Error:', error)
  }
}

function confirmarEliminar(id) {
  alertaToDelete.value = id
  showDeleteModal.value = true
}

async function eliminarAlerta() {
  if (!alertaToDelete.value) return
  deleting.value = true
  try {
    await alertaService.eliminarAlerta(alertaToDelete.value)
    toast.success('Alerta eliminada correctamente')
    await cargarAlertas()
    showDeleteModal.value = false
  } catch (error) {
    toast.error('Error al eliminar la alerta')
    console.error('Error:', error)
  } finally {
    deleting.value = false
  }
}

onMounted(() => {
  cargarAlertas()
})
</script>

<style scoped>
</style> 