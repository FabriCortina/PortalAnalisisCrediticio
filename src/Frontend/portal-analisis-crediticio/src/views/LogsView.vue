<template>
  <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
    <!-- Encabezado -->
    <div class="mb-8">
      <h1 class="text-2xl font-bold text-gray-900">Logs del Sistema</h1>
      <p class="mt-1 text-sm text-gray-500">
        Registro de actividades del sistema
      </p>
    </div>

    <!-- Filtros -->
    <div class="bg-white p-4 rounded-lg shadow mb-6">
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <!-- Filtro por usuario -->
        <div>
          <label for="usuario" class="block text-sm font-medium text-gray-700">Usuario</label>
          <select
            id="usuario"
            v-model="filtros.usuario"
            class="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm rounded-md"
          >
            <option value="">Todos</option>
            <option v-for="usuario in usuarios" :key="usuario.id" :value="usuario.id">
              {{ usuario.nombre }}
            </option>
          </select>
        </div>

        <!-- Filtro por tipo de acción -->
        <div>
          <label for="tipoAccion" class="block text-sm font-medium text-gray-700">Tipo de Acción</label>
          <select
            id="tipoAccion"
            v-model="filtros.tipoAccion"
            class="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm rounded-md"
          >
            <option value="">Todos</option>
            <option v-for="tipo in tiposAccion" :key="tipo.id" :value="tipo.id">
              {{ tipo.nombre }}
            </option>
          </select>
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

    <!-- Tabla de logs -->
    <div v-else class="bg-white shadow overflow-hidden sm:rounded-md">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50">
          <tr>
            <th
              v-for="columna in columnas"
              :key="columna.id"
              scope="col"
              class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer"
              @click="ordenarPor(columna.id)"
            >
              <div class="flex items-center">
                {{ columna.nombre }}
                <span v-if="ordenarPor === columna.id" class="ml-1">
                  {{ orden === 'asc' ? '↑' : '↓' }}
                </span>
              </div>
            </th>
          </tr>
        </thead>
        <tbody class="bg-white divide-y divide-gray-200">
          <tr v-for="log in logs" :key="log.id" class="hover:bg-gray-50">
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
              {{ formatoFecha(log.fecha) }}
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
              {{ log.usuario }}
            </td>
            <td class="px-6 py-4 whitespace-nowrap">
              <div class="flex items-center">
                <span
                  class="px-2 inline-flex text-xs leading-5 font-semibold rounded-full"
                  :class="getColorAccion(log.tipoAccion)"
                >
                  {{ log.tipoAccion }}
                </span>
                <div class="ml-2">
                  <div
                    class="group relative"
                    v-if="getDescripcionAccion(log.tipoAccion)"
                  >
                    <svg
                      class="h-4 w-4 text-gray-400 cursor-help"
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 20 20"
                      fill="currentColor"
                    >
                      <path
                        fill-rule="evenodd"
                        d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-8-3a1 1 0 00-.867.5 1 1 0 11-1.731-1A3 3 0 0113 8a3.001 3.001 0 01-2 2.83V11a1 1 0 11-2 0v-1a1 1 0 011-1 1 1 0 100-2zm0 8a1 1 0 100-2 1 1 0 000 2z"
                        clip-rule="evenodd"
                      />
                    </svg>
                    <div class="hidden group-hover:block absolute z-10 w-64 p-2 mt-1 text-sm text-white bg-gray-900 rounded-lg shadow-lg">
                      {{ getDescripcionAccion(log.tipoAccion) }}
                    </div>
                  </div>
                </div>
              </div>
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
              {{ log.entidad }}
            </td>
            <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
              {{ log.detalles }}
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Paginación -->
      <div class="bg-white px-4 py-3 flex items-center justify-between border-t border-gray-200 sm:px-6">
        <div class="flex-1 flex justify-between sm:hidden">
          <button
            @click="cambiarPagina(pagina - 1)"
            :disabled="pagina === 1"
            class="relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
          >
            Anterior
          </button>
          <button
            @click="cambiarPagina(pagina + 1)"
            :disabled="pagina === totalPaginas"
            class="ml-3 relative inline-flex items-center px-4 py-2 border border-gray-300 text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50"
          >
            Siguiente
          </button>
        </div>
        <div class="hidden sm:flex-1 sm:flex sm:items-center sm:justify-between">
          <div>
            <p class="text-sm text-gray-700">
              Mostrando
              <span class="font-medium">{{ (pagina - 1) * porPagina + 1 }}</span>
              a
              <span class="font-medium">{{ Math.min(pagina * porPagina, totalRegistros) }}</span>
              de
              <span class="font-medium">{{ totalRegistros }}</span>
              resultados
            </p>
          </div>
          <div>
            <nav class="relative z-0 inline-flex rounded-md shadow-sm -space-x-px" aria-label="Pagination">
              <button
                @click="cambiarPagina(pagina - 1)"
                :disabled="pagina === 1"
                class="relative inline-flex items-center px-2 py-2 rounded-l-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
              >
                <span class="sr-only">Anterior</span>
                <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                  <path fill-rule="evenodd" d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z" clip-rule="evenodd" />
                </svg>
              </button>
              <button
                v-for="pag in paginasAMostrar"
                :key="pag"
                @click="cambiarPagina(pag)"
                :class="[
                  pag === pagina
                    ? 'z-10 bg-blue-50 border-blue-500 text-blue-600'
                    : 'bg-white border-gray-300 text-gray-500 hover:bg-gray-50',
                  'relative inline-flex items-center px-4 py-2 border text-sm font-medium'
                ]"
              >
                {{ pag }}
              </button>
              <button
                @click="cambiarPagina(pagina + 1)"
                :disabled="pagina === totalPaginas"
                class="relative inline-flex items-center px-2 py-2 rounded-r-md border border-gray-300 bg-white text-sm font-medium text-gray-500 hover:bg-gray-50"
              >
                <span class="sr-only">Siguiente</span>
                <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                  <path fill-rule="evenodd" d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z" clip-rule="evenodd" />
                </svg>
              </button>
            </nav>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { logService } from '@/services/logService'
import { useToast } from '@/composables/useToast'

const { showToast } = useToast()

// Estado
const loading = ref(false)
const error = ref(null)
const logs = ref([])
const usuarios = ref([])
const tiposAccion = ref([])
const totalRegistros = ref(0)
const pagina = ref(1)
const porPagina = ref(10)
const ordenarPor = ref('fecha')
const orden = ref('desc')

// Filtros
const filtros = ref({
  usuario: '',
  tipoAccion: '',
  fechaInicio: '',
  fechaFin: ''
})

// Columnas de la tabla
const columnas = [
  { id: 'fecha', nombre: 'Fecha' },
  { id: 'usuario', nombre: 'Usuario' },
  { id: 'tipoAccion', nombre: 'Acción' },
  { id: 'entidad', nombre: 'Entidad' },
  { id: 'detalles', nombre: 'Detalles' }
]

// Computed
const totalPaginas = computed(() => Math.ceil(totalRegistros.value / porPagina.value))

const paginasAMostrar = computed(() => {
  const paginas = []
  const maxPaginas = 5
  let inicio = Math.max(1, pagina.value - Math.floor(maxPaginas / 2))
  let fin = Math.min(totalPaginas.value, inicio + maxPaginas - 1)

  if (fin - inicio + 1 < maxPaginas) {
    inicio = Math.max(1, fin - maxPaginas + 1)
  }

  for (let i = inicio; i <= fin; i++) {
    paginas.push(i)
  }

  return paginas
})

// Funciones
function formatoFecha(fecha) {
  return new Date(fecha).toLocaleString('es-AR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

function getColorAccion(tipoAccion) {
  const colores = {
    'CREAR': 'bg-green-100 text-green-800',
    'ACTUALIZAR': 'bg-blue-100 text-blue-800',
    'ELIMINAR': 'bg-red-100 text-red-800',
    'CONSULTAR': 'bg-gray-100 text-gray-800',
    'APROBAR': 'bg-purple-100 text-purple-800',
    'RECHAZAR': 'bg-yellow-100 text-yellow-800'
  }
  return colores[tipoAccion] || 'bg-gray-100 text-gray-800'
}

function getDescripcionAccion(tipoAccion) {
  const descripciones = {
    'CREAR': 'Se ha creado un nuevo registro en el sistema',
    'ACTUALIZAR': 'Se ha modificado un registro existente',
    'ELIMINAR': 'Se ha eliminado un registro del sistema',
    'CONSULTAR': 'Se ha realizado una consulta de información',
    'APROBAR': 'Se ha aprobado una solicitud o documento',
    'RECHAZAR': 'Se ha rechazado una solicitud o documento'
  }
  return descripciones[tipoAccion]
}

async function cargarLogs() {
  loading.value = true
  error.value = null

  try {
    const data = await logService.getLogs(
      filtros.value,
      pagina.value,
      porPagina.value,
      ordenarPor.value,
      orden.value
    )
    logs.value = data.items
    totalRegistros.value = data.total
  } catch (err) {
    error.value = 'Error al cargar los logs'
    showToast('Error al cargar los logs', 'error')
  } finally {
    loading.value = false
  }
}

async function cargarDatosIniciales() {
  try {
    const [usuariosData, tiposAccionData] = await Promise.all([
      logService.getUsuarios(),
      logService.getTiposAccion()
    ])
    usuarios.value = usuariosData
    tiposAccion.value = tiposAccionData
  } catch (err) {
    showToast('Error al cargar datos iniciales', 'error')
  }
}

function cambiarPagina(nuevaPagina) {
  if (nuevaPagina >= 1 && nuevaPagina <= totalPaginas.value) {
    pagina.value = nuevaPagina
  }
}

function ordenarPor(columna) {
  if (ordenarPor.value === columna) {
    orden.value = orden.value === 'asc' ? 'desc' : 'asc'
  } else {
    ordenarPor.value = columna
    orden.value = 'asc'
  }
}

// Observar cambios en filtros
watch(filtros, () => {
  pagina.value = 1
  cargarLogs()
}, { deep: true })

// Observar cambios en paginación y ordenamiento
watch([pagina, ordenarPor, orden], () => {
  cargarLogs()
})

// Cargar datos iniciales
onMounted(async () => {
  await cargarDatosIniciales()
  await cargarLogs()
})
</script> 