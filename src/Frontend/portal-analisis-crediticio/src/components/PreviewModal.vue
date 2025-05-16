<template>
  <div class="fixed inset-0 z-50 overflow-y-auto">
    <div class="flex items-center justify-center min-h-screen px-4 pt-4 pb-20 text-center sm:block sm:p-0">
      <!-- Fondo oscuro -->
      <div class="fixed inset-0 transition-opacity" aria-hidden="true">
        <div class="absolute inset-0 bg-gray-500 opacity-75"></div>
      </div>

      <!-- Modal -->
      <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-6xl sm:w-full">
        <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
          <div class="sm:flex sm:items-start">
            <div class="mt-3 text-center sm:mt-0 sm:text-left w-full">
              <!-- Encabezado -->
              <div class="flex justify-between items-center mb-4">
                <div>
                  <h3 class="text-lg leading-6 font-medium text-gray-900">
                    {{ archivo.nombre }}
                  </h3>
                  <p class="text-sm text-gray-500">
                    {{ archivo.tipo }} · {{ formatearTamaño(archivo.tamaño) }}
                  </p>
                </div>
                <div class="flex items-center space-x-2">
                  <button
                    type="button"
                    class="text-gray-400 hover:text-gray-500"
                    @click="descargarArchivo"
                  >
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                    </svg>
                  </button>
                  <button
                    type="button"
                    class="text-gray-400 hover:text-gray-500"
                    @click="$emit('close')"
                  >
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                    </svg>
                  </button>
                </div>
              </div>

              <!-- Contenido -->
              <div class="mt-4">
                <!-- PDF -->
                <div v-if="esPDF" class="w-full h-[600px]">
                  <iframe
                    :src="urlPrevisualizacion"
                    class="w-full h-full border-0"
                    type="application/pdf"
                  ></iframe>
                </div>

                <!-- Imagen -->
                <div v-else-if="esImagen" class="flex justify-center">
                  <img
                    :src="urlPrevisualizacion"
                    :alt="archivo.nombre"
                    class="max-h-[600px] object-contain"
                  />
                </div>

                <!-- Documento -->
                <div v-else-if="esDocumento" class="w-full h-[600px]">
                  <iframe
                    :src="urlPrevisualizacion"
                    class="w-full h-full border-0"
                  ></iframe>
                </div>

                <!-- Hoja de cálculo -->
                <div v-else-if="esHojaCalculo" class="w-full h-[600px]">
                  <iframe
                    :src="urlPrevisualizacion"
                    class="w-full h-full border-0"
                  ></iframe>
                </div>

                <!-- No soportado -->
                <div v-else class="flex flex-col items-center justify-center h-[400px] text-gray-500">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                  </svg>
                  <p class="text-lg font-medium">Vista previa no disponible</p>
                  <p class="text-sm mt-2">
                    Este tipo de archivo no puede ser previsualizado.
                    <button
                      type="button"
                      class="text-blue-600 hover:text-blue-500"
                      @click="descargarArchivo"
                    >
                      Descargar archivo
                    </button>
                  </p>
                </div>
              </div>

              <!-- Metadatos -->
              <div class="mt-6 border-t border-gray-200 pt-4">
                <dl class="grid grid-cols-1 gap-x-4 gap-y-4 sm:grid-cols-2">
                  <div>
                    <dt class="text-sm font-medium text-gray-500">Subido por</dt>
                    <dd class="mt-1 text-sm text-gray-900">{{ archivo.usuario }}</dd>
                  </div>
                  <div>
                    <dt class="text-sm font-medium text-gray-500">Fecha de subida</dt>
                    <dd class="mt-1 text-sm text-gray-900">{{ formatearFecha(archivo.fecha) }}</dd>
                  </div>
                  <div>
                    <dt class="text-sm font-medium text-gray-500">Categoría</dt>
                    <dd class="mt-1 text-sm text-gray-900">{{ archivo.categoria }}</dd>
                  </div>
                  <div>
                    <dt class="text-sm font-medium text-gray-500">Descripción</dt>
                    <dd class="mt-1 text-sm text-gray-900">{{ archivo.descripcion || 'Sin descripción' }}</dd>
                  </div>
                </dl>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useToast } from 'vue-toastification'
import { clienteService } from '../services/clienteService'

const props = defineProps({
  archivo: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['close'])
const toast = useToast()
const urlPrevisualizacion = ref('')

// Computed properties para tipo de archivo
const esPDF = computed(() => props.archivo.tipo === 'application/pdf')
const esImagen = computed(() => props.archivo.tipo.startsWith('image/'))
const esDocumento = computed(() => props.archivo.tipo === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document')
const esHojaCalculo = computed(() => props.archivo.tipo === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet')

// Funciones
function formatearTamaño(bytes) {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const sizes = ['Bytes', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i]
}

function formatearFecha(fecha) {
  return new Date(fecha).toLocaleString('es-AR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

async function descargarArchivo() {
  try {
    const url = await clienteService.obtenerUrlDescarga(props.archivo.id)
    window.open(url, '_blank')
  } catch (err) {
    console.error('Error al descargar archivo:', err)
    toast.error('Error al descargar el archivo')
  }
}

// Cargar URL de previsualización
async function cargarUrlPrevisualizacion() {
  try {
    urlPrevisualizacion.value = await clienteService.obtenerUrlPrevisualizacion(props.archivo.id)
  } catch (err) {
    console.error('Error al cargar previsualización:', err)
    toast.error('Error al cargar la previsualización')
  }
}

onMounted(cargarUrlPrevisualizacion)
</script> 