<template>
  <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
    <!-- Encabezado -->
    <div class="mb-8 flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Créditos Activos</h1>
        <p class="mt-1 text-sm text-gray-500">
          Gestión de créditos activos del sistema
        </p>
      </div>
      <div class="flex space-x-4">
        <ExportButton
          :columnas="columnasExportacion"
          :datos="creditos"
          nombre-archivo="creditos-activos"
        />
      </div>
    </div>

    <!-- Filtros -->
    <div class="bg-white p-4 rounded-lg shadow mb-6">
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <!-- Filtro por estado -->
        <div>
          <label for="estado" class="block text-sm font-medium text-gray-700">Estado</label>
          <select
            id="estado"
            v-model="filtros.estado"
            class="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm rounded-md"
          >
            <option value="">Todos</option>
            <option value="pagado">Pagado</option>
            <option value="pendiente">Pendiente</option>
            <option value="vencido">Vencido</option>
          </select>
        </div>

        <!-- Filtro por cliente -->
        <div>
          <label for="cliente" class="block text-sm font-medium text-gray-700">Cliente</label>
          <input
            type="text"
            id="cliente"
            v-model="filtros.cliente"
            placeholder="Buscar por nombre o CUIT"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
          />
        </div>

        <!-- Filtro por fecha inicio -->
        <div>
          <label for="fechaInicio" class="block text-sm font-medium text-gray-700">Fecha Inicio</label>
          <input
            type="date"
            id="fechaInicio"
            v-model="filtros.fechaInicio"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
          />
        </div>

        <!-- Filtro por fecha fin -->
        <div>
          <label for="fechaFin" class="block text-sm font-medium text-gray-700">Fecha Fin</label>
          <input
            type="date"
            id="fechaFin"
            v-model="filtros.fechaFin"
            class="mt-1 block w-full border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
          />
        </div>
      </div>
    </div>

    <!-- Estado de carga -->
    <div v-if="loading" class="flex justify-center items-center py-8">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
    </div>

    <!-- Mensaje de error -->
    <div v-else-if="error" class="bg-red-50 border-l-4 border-red-400 p-4 mb-6">
      <div class="flex">
        <div class="flex-shrink-0">
          <svg class="h-5 w-5 text-red-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clip-rule="evenodd" />
          </svg>
        </div>
        <div class="ml-3">
          <p class="text-sm text-red-700">{{ error }}</p>
        </div>
      </div>
    </div>

    <!-- Tabla de créditos -->
    <div v-else class="bg-white shadow overflow-hidden sm:rounded-md">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Cliente
            </th>
            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Monto
            </th>
            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Estado
            </th>
            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Próximo Vencimiento
            </th>
            <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
              Acciones
            </th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="credito in creditosFiltrados" :key="credito.id">
            <td class="px-6 py-4 whitespace-nowrap">
              <div class="flex items-center">
                <div class="flex-shrink-0 h-10 w-10">
                  <div class="h-10 w-10 rounded-full bg-gray-200 flex items-center justify-center">
                    <span class="text-gray-500 font-medium">{{ credito.cliente.iniciales }}</span>
                  </div>
                </div>
                <div class="ml-4">
                  <div class="text-sm font-medium text-gray-900">
                    {{ credito.cliente.nombre }}
                  </div>
                  <div class="text-sm text-gray-500">
                    {{ credito.cliente.cuit }}
                  </div>
                </div>
              </div>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
              <div class="text-sm text-gray-900">${{ credito.monto.toLocaleString('es-AR') }}</div>
              <div class="text-sm text-gray-500">{{ credito.cuotasPagadas }}/{{ credito.cuotasTotales }} cuotas</div>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
              <span
                class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full"
                :class="{
                  'bg-green-100 text-green-800': credito.estado === 'pagado',
                  'bg-yellow-100 text-yellow-800': credito.estado === 'pendiente',
                  'bg-red-100 text-red-800': credito.estado === 'vencido'
                }"
              >
                {{ credito.estado.charAt(0).toUpperCase() + credito.estado.slice(1) }}
              </span>
              <div v-if="credito.cuotasVencidas > 0" class="mt-1 text-xs text-red-600">
                {{ credito.cuotasVencidas }} cuota(s) vencida(s)
              </div>
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
              <div class="text-sm text-gray-900">
                {{ formatoFecha(credito.proximoVencimiento) }}
              </div>
              <div
                v-if="credito.diasParaVencimiento <= 7 && credito.diasParaVencimiento > 0"
                class="text-xs text-yellow-600"
              >
                Vence en {{ credito.diasParaVencimiento }} días
              </div>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
              <button
                @click="toggleDetalle(credito.id)"
                class="text-blue-600 hover:text-blue-900"
              >
                {{ creditoSeleccionado?.id === credito.id ? 'Ocultar' : 'Ver' }} detalles
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal de detalles -->
    <div
      v-if="creditoSeleccionado"
      class="fixed inset-0 z-50 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div
          class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"
          aria-hidden="true"
          @click="cerrarDetalle"
        ></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-2xl sm:w-full">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div class="mt-3 text-center sm:mt-0 sm:text-left w-full">
                <h3 class="text-lg leading-6 font-medium text-gray-900" id="modal-title">
                  Detalles del Crédito
                </h3>
                
                <!-- Información del cliente -->
                <div class="mt-4 border-t border-gray-200 pt-4">
                  <h4 class="text-sm font-medium text-gray-900">Cliente</h4>
                  <div class="mt-2 grid grid-cols-2 gap-4">
                    <div>
                      <p class="text-sm text-gray-500">Nombre</p>
                      <p class="mt-1 text-sm text-gray-900">{{ creditoSeleccionado.cliente.nombre }}</p>
                    </div>
                    <div>
                      <p class="text-sm text-gray-500">CUIT</p>
                      <p class="mt-1 text-sm text-gray-900">{{ creditoSeleccionado.cliente.cuit }}</p>
                    </div>
                  </div>
                </div>

                <!-- Condiciones del crédito -->
                <div class="mt-4 border-t border-gray-200 pt-4">
                  <h4 class="text-sm font-medium text-gray-900">Condiciones</h4>
                  <div class="mt-2 grid grid-cols-2 gap-4">
                    <div>
                      <p class="text-sm text-gray-500">Monto</p>
                      <p class="mt-1 text-sm text-gray-900">${{ creditoSeleccionado.monto.toLocaleString('es-AR') }}</p>
                    </div>
                    <div>
                      <p class="text-sm text-gray-500">Tasa Anual</p>
                      <p class="mt-1 text-sm text-gray-900">{{ creditoSeleccionado.tasaAnual }}%</p>
                    </div>
                    <div>
                      <p class="text-sm text-gray-500">Plazo</p>
                      <p class="mt-1 text-sm text-gray-900">{{ creditoSeleccionado.plazo }} meses</p>
                    </div>
                    <div>
                      <p class="text-sm text-gray-500">Frecuencia de Pago</p>
                      <p class="mt-1 text-sm text-gray-900">{{ creditoSeleccionado.frecuenciaPago }}</p>
                    </div>
                  </div>
                </div>

                <!-- Cuotas -->
                <div class="mt-4 border-t border-gray-200 pt-4">
                  <h4 class="text-sm font-medium text-gray-900">Cuotas</h4>
                  <div class="mt-2">
                    <table class="min-w-full divide-y divide-gray-200">
                      <thead class="bg-gray-50">
                        <tr>
                          <th scope="col" class="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Número
                          </th>
                          <th scope="col" class="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Vencimiento
                          </th>
                          <th scope="col" class="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Monto
                          </th>
                          <th scope="col" class="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                            Estado
                          </th>
                        </tr>
                      </thead>
                      <tbody class="bg-white divide-y divide-gray-200">
                        <tr v-for="cuota in creditoSeleccionado.cuotas" :key="cuota.numero">
                          <td class="px-3 py-2 whitespace-nowrap text-sm text-gray-900">
                            {{ cuota.numero }}
                          </td>
                          <td class="px-3 py-2 whitespace-nowrap text-sm text-gray-900">
                            {{ formatoFecha(cuota.vencimiento) }}
                          </td>
                          <td class="px-3 py-2 whitespace-nowrap text-sm text-gray-900">
                            ${{ cuota.monto.toLocaleString('es-AR') }}
                          </td>
                          <td class="px-3 py-2 whitespace-nowrap">
                            <span
                              class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full"
                              :class="{
                                'bg-green-100 text-green-800': cuota.estado === 'pagado',
                                'bg-yellow-100 text-yellow-800': cuota.estado === 'pendiente',
                                'bg-red-100 text-red-800': cuota.estado === 'vencido'
                              }"
                            >
                              {{ cuota.estado.charAt(0).toUpperCase() + cuota.estado.slice(1) }}
                            </span>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button
              type="button"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
              @click="cerrarDetalle"
            >
              Cerrar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Notificación de vencimiento próximo -->
    <div
      v-if="creditosProximosAVencer.length > 0"
      class="fixed bottom-4 right-4 bg-yellow-50 border-l-4 border-yellow-400 p-4"
    >
      <div class="flex">
        <div class="flex-shrink-0">
          <svg class="h-5 w-5 text-yellow-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd" />
          </svg>
        </div>
        <div class="ml-3">
          <p class="text-sm text-yellow-700">
            {{ creditosProximosAVencer.length }} crédito(s) próximo(s) a vencer
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { creditoService } from '@/services/creditoService'
import { useToast } from '@/composables/useToast'
import ExportButton from '@/components/ExportButton.vue'

const { showToast } = useToast()

// Estado
const filtros = ref({
  estado: '',
  cliente: '',
  fechaInicio: '',
  fechaFin: ''
})

const creditoSeleccionado = ref(null)
const creditos = ref([])
const loading = ref(false)
const error = ref(null)

// Columnas para exportación
const columnasExportacion = [
  { id: 'cliente', nombre: 'Cliente' },
  { id: 'cuit', nombre: 'CUIT' },
  { id: 'monto', nombre: 'Monto' },
  { id: 'estado', nombre: 'Estado' },
  { id: 'fechaInicio', nombre: 'Fecha de Inicio' },
  { id: 'fechaVencimiento', nombre: 'Fecha de Vencimiento' },
  { id: 'cuotasPagadas', nombre: 'Cuotas Pagadas' },
  { id: 'cuotasTotales', nombre: 'Cuotas Totales' },
  { id: 'proximaCuota', nombre: 'Próxima Cuota' },
  { id: 'montoProximaCuota', nombre: 'Monto Próxima Cuota' }
]

// Cargar créditos
async function cargarCreditos() {
  loading.value = true
  error.value = null
  
  try {
    const data = await creditoService.getCreditosActivos(filtros.value)
    creditos.value = data
  } catch (err) {
    error.value = 'Error al cargar los créditos'
    showToast('Error al cargar los créditos', 'error')
  } finally {
    loading.value = false
  }
}

// Cargar detalle del crédito
async function cargarDetalleCredito(id) {
  try {
    const data = await creditoService.getCreditoDetalle(id)
    creditoSeleccionado.value = data
  } catch (err) {
    showToast('Error al cargar el detalle del crédito', 'error')
    cerrarDetalle()
  }
}

// Computed
const creditosFiltrados = computed(() => {
  return creditos.value.filter(credito => {
    const cumpleEstado = !filtros.value.estado || credito.estado === filtros.value.estado
    const cumpleCliente = !filtros.value.cliente || 
      credito.cliente.nombre.toLowerCase().includes(filtros.value.cliente.toLowerCase()) ||
      credito.cliente.cuit.includes(filtros.value.cliente)
    const cumpleFechaInicio = !filtros.value.fechaInicio || 
      new Date(credito.proximoVencimiento) >= new Date(filtros.value.fechaInicio)
    const cumpleFechaFin = !filtros.value.fechaFin || 
      new Date(credito.proximoVencimiento) <= new Date(filtros.value.fechaFin)

    return cumpleEstado && cumpleCliente && cumpleFechaInicio && cumpleFechaFin
  })
})

const creditosProximosAVencer = computed(() => {
  return creditos.value.filter(credito => 
    credito.diasParaVencimiento > 0 && credito.diasParaVencimiento <= 7
  )
})

// Funciones
async function toggleDetalle(id) {
  if (creditoSeleccionado.value?.id === id) {
    cerrarDetalle()
  } else {
    await cargarDetalleCredito(id)
  }
}

function cerrarDetalle() {
  creditoSeleccionado.value = null
}

function formatoFecha(fecha) {
  return new Date(fecha).toLocaleDateString('es-AR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

// Observar cambios en filtros
watch(filtros, () => {
  cargarCreditos()
}, { deep: true })

// Cargar datos iniciales
onMounted(async () => {
  await cargarCreditos()
  
  // Configurar polling para créditos próximos a vencer
  setInterval(async () => {
    try {
      const proximosVencer = await creditoService.getCreditosProximosAVencer()
      if (proximosVencer.length > 0) {
        showToast(`${proximosVencer.length} crédito(s) próximo(s) a vencer`, 'warning')
      }
    } catch (err) {
      console.error('Error al verificar créditos próximos a vencer:', err)
    }
  }, 5 * 60 * 1000) // Cada 5 minutos
})
</script>

<style>
/* Estilos para impresión */
@media print {
  .no-print {
    display: none !important;
  }
}
</style> 