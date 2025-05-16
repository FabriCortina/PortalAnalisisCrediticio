<template>
  <div class="relative">
    <!-- Botón de exportación -->
    <button
      @click="mostrarModal = true"
      class="inline-flex items-center px-4 py-2 border border-gray-300 shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
    >
      <svg
        class="-ml-1 mr-2 h-5 w-5 text-gray-500"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 20 20"
        fill="currentColor"
      >
        <path
          fill-rule="evenodd"
          d="M3 17a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm3.293-7.707a1 1 0 011.414 0L9 10.586V3a1 1 0 112 0v7.586l1.293-1.293a1 1 0 111.414 1.414l-3 3a1 1 0 01-1.414 0l-3-3a1 1 0 010-1.414z"
          clip-rule="evenodd"
        />
      </svg>
      Exportar
    </button>

    <!-- Modal de exportación -->
    <div
      v-if="mostrarModal"
      class="fixed z-10 inset-0 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div
          class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"
          aria-hidden="true"
          @click="mostrarModal = false"
        ></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <div
          class="inline-block align-bottom bg-white rounded-lg px-4 pt-5 pb-4 text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full sm:p-6"
        >
          <div>
            <div class="mt-3 text-center sm:mt-5">
              <h3 class="text-lg leading-6 font-medium text-gray-900" id="modal-title">
                Exportar Datos
              </h3>
              <div class="mt-4">
                <!-- Formato de exportación -->
                <div class="mb-4">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Formato</label>
                  <div class="flex space-x-4">
                    <button
                      @click="formatoSeleccionado = 'csv'"
                      :class="[
                        formatoSeleccionado === 'csv'
                          ? 'bg-blue-100 text-blue-700'
                          : 'bg-gray-100 text-gray-700',
                        'px-4 py-2 rounded-md text-sm font-medium'
                      ]"
                    >
                      CSV
                    </button>
                    <button
                      @click="formatoSeleccionado = 'excel'"
                      :class="[
                        formatoSeleccionado === 'excel'
                          ? 'bg-blue-100 text-blue-700'
                          : 'bg-gray-100 text-gray-700',
                        'px-4 py-2 rounded-md text-sm font-medium'
                      ]"
                    >
                      Excel
                    </button>
                  </div>
                </div>

                <!-- Selector de columnas -->
                <div class="mb-4">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Columnas a incluir</label>
                  <div class="space-y-2 max-h-60 overflow-y-auto">
                    <div v-for="columna in columnas" :key="columna.id" class="flex items-center">
                      <input
                        :id="columna.id"
                        type="checkbox"
                        v-model="columnasSeleccionadas"
                        :value="columna.id"
                        class="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                      />
                      <label :for="columna.id" class="ml-2 block text-sm text-gray-900">
                        {{ columna.nombre }}
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="mt-5 sm:mt-6 sm:grid sm:grid-cols-2 sm:gap-3 sm:grid-flow-row-dense">
            <button
              type="button"
              @click="exportarDatos"
              class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:col-start-2 sm:text-sm"
              :disabled="exportando"
            >
              <svg
                v-if="exportando"
                class="animate-spin -ml-1 mr-3 h-5 w-5 text-white"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
              >
                <circle
                  class="opacity-25"
                  cx="12"
                  cy="12"
                  r="10"
                  stroke="currentColor"
                  stroke-width="4"
                ></circle>
                <path
                  class="opacity-75"
                  fill="currentColor"
                  d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                ></path>
              </svg>
              {{ exportando ? 'Exportando...' : 'Exportar' }}
            </button>
            <button
              type="button"
              @click="mostrarModal = false"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:col-start-1 sm:text-sm"
            >
              Cancelar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Notificación de éxito -->
    <div
      v-if="archivoGenerado"
      class="fixed bottom-4 right-4 bg-green-50 border-l-4 border-green-400 p-4 rounded shadow-lg"
    >
      <div class="flex">
        <div class="flex-shrink-0">
          <svg class="h-5 w-5 text-green-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path
              fill-rule="evenodd"
              d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
              clip-rule="evenodd"
            />
          </svg>
        </div>
        <div class="ml-3">
          <p class="text-sm text-green-700">
            Archivo generado correctamente.
            <a
              :href="urlDescarga"
              download
              class="font-medium underline text-green-700 hover:text-green-600"
            >
              Descargar
            </a>
          </p>
        </div>
        <div class="ml-auto pl-3">
          <div class="-mx-1.5 -my-1.5">
            <button
              @click="archivoGenerado = false"
              class="inline-flex rounded-md p-1.5 text-green-500 hover:bg-green-100 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
            >
              <span class="sr-only">Cerrar</span>
              <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                <path
                  fill-rule="evenodd"
                  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                  clip-rule="evenodd"
                />
              </svg>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useToast } from '@/composables/useToast'
import * as XLSX from 'xlsx'

const props = defineProps({
  columnas: {
    type: Array,
    required: true
  },
  datos: {
    type: Array,
    required: true
  },
  nombreArchivo: {
    type: String,
    required: true
  }
})

const { showToast } = useToast()

const mostrarModal = ref(false)
const formatoSeleccionado = ref('csv')
const columnasSeleccionadas = ref([])
const exportando = ref(false)
const archivoGenerado = ref(false)
const urlDescarga = ref('')

// Inicializar columnas seleccionadas
columnasSeleccionadas.value = props.columnas.map(col => col.id)

async function exportarDatos() {
  if (columnasSeleccionadas.value.length === 0) {
    showToast('Selecciona al menos una columna', 'error')
    return
  }

  exportando.value = true

  try {
    // Filtrar datos según columnas seleccionadas
    const datosFiltrados = props.datos.map(item => {
      const nuevoItem = {}
      columnasSeleccionadas.value.forEach(colId => {
        nuevoItem[colId] = item[colId]
      })
      return nuevoItem
    })

    // Convertir a CSV o Excel según el formato seleccionado
    let contenido
    let extension
    let tipoMIME

    if (formatoSeleccionado.value === 'csv') {
      contenido = convertirACSV(datosFiltrados)
      extension = 'csv'
      tipoMIME = 'text/csv'
    } else {
      contenido = await convertirAExcel(datosFiltrados)
      extension = 'xlsx'
      tipoMIME = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    }

    // Crear blob y URL para descarga
    const blob = new Blob([contenido], { type: tipoMIME })
    urlDescarga.value = URL.createObjectURL(blob)

    // Mostrar notificación de éxito
    archivoGenerado.value = true
    mostrarModal.value = false

    // Descargar archivo automáticamente
    const link = document.createElement('a')
    link.href = urlDescarga.value
    link.download = `${props.nombreArchivo}.${extension}`
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)

  } catch (error) {
    showToast('Error al exportar los datos', 'error')
  } finally {
    exportando.value = false
  }
}

function convertirACSV(datos) {
  // Obtener encabezados
  const encabezados = columnasSeleccionadas.value.map(colId => {
    const columna = props.columnas.find(col => col.id === colId)
    return columna.nombre
  })

  // Crear filas
  const filas = datos.map(item => {
    return columnasSeleccionadas.value.map(colId => {
      const valor = item[colId]
      // Escapar comas y comillas
      return typeof valor === 'string' && (valor.includes(',') || valor.includes('"'))
        ? `"${valor.replace(/"/g, '""')}"`
        : valor
    }).join(',')
  })

  // Unir todo
  return [encabezados.join(','), ...filas].join('\n')
}

async function convertirAExcel(datos) {
  // Crear un nuevo libro de Excel
  const wb = XLSX.utils.book_new()
  
  // Convertir los datos a una hoja de cálculo
  const ws = XLSX.utils.json_to_sheet(datos)
  
  // Agregar la hoja al libro
  XLSX.utils.book_append_sheet(wb, ws, 'Datos')
  
  // Generar el archivo Excel
  return XLSX.write(wb, { type: 'array', bookType: 'xlsx' })
}
</script> 