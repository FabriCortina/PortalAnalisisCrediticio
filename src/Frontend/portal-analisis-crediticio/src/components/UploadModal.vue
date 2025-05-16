<template>
  <div class="fixed inset-0 z-50 overflow-y-auto">
    <div class="flex items-center justify-center min-h-screen px-4 pt-4 pb-20 text-center sm:block sm:p-0">
      <!-- Fondo oscuro -->
      <div class="fixed inset-0 transition-opacity" aria-hidden="true">
        <div class="absolute inset-0 bg-gray-500 opacity-75"></div>
      </div>

      <!-- Modal -->
      <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
        <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
          <div class="sm:flex sm:items-start">
            <div class="mt-3 text-center sm:mt-0 sm:text-left w-full">
              <h3 class="text-lg leading-6 font-medium text-gray-900 mb-4">
                Subir Archivo
              </h3>

              <!-- Área de drag & drop -->
              <div
                class="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center"
                :class="{ 'border-blue-500 bg-blue-50': isDragging }"
                @dragenter.prevent="isDragging = true"
                @dragleave.prevent="isDragging = false"
                @dragover.prevent
                @drop.prevent="onDrop"
              >
                <div v-if="!archivoSeleccionado">
                  <svg xmlns="http://www.w3.org/2000/svg" class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" />
                  </svg>
                  <p class="mt-2 text-sm text-gray-600">
                    Arrastra y suelta tu archivo aquí, o
                    <button
                      type="button"
                      class="text-blue-600 hover:text-blue-500"
                      @click="seleccionarArchivo"
                    >
                      selecciona un archivo
                    </button>
                  </p>
                  <p class="mt-1 text-xs text-gray-500">
                    PDF, Word, Excel, imágenes (máx. 10MB)
                  </p>
                </div>

                <!-- Vista previa del archivo -->
                <div v-else class="space-y-4">
                  <div class="flex items-center justify-between">
                    <div class="flex items-center space-x-3">
                      <component :is="obtenerIconoArchivo(archivoSeleccionado.type)" class="h-8 w-8 text-gray-500" />
                      <div class="text-left">
                        <p class="text-sm font-medium text-gray-900">{{ archivoSeleccionado.name }}</p>
                        <p class="text-xs text-gray-500">{{ formatearTamaño(archivoSeleccionado.size) }}</p>
                      </div>
                    </div>
                    <button
                      type="button"
                      class="text-gray-400 hover:text-gray-500"
                      @click="archivoSeleccionado = null"
                    >
                      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                      </svg>
                    </button>
                  </div>

                  <!-- Categoría -->
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">
                      Categoría
                    </label>
                    <select
                      v-model="categoria"
                      class="w-full rounded-lg border-gray-300 focus:ring-blue-500 focus:border-blue-500"
                    >
                      <option value="documentos">Documentos</option>
                      <option value="entrevistas">Entrevistas</option>
                      <option value="garantias">Garantías</option>
                      <option value="analisis">Análisis</option>
                    </select>
                  </div>

                  <!-- Descripción -->
                  <div>
                    <label class="block text-sm font-medium text-gray-700 mb-1">
                      Descripción
                    </label>
                    <textarea
                      v-model="descripcion"
                      rows="3"
                      class="w-full rounded-lg border-gray-300 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Agrega una descripción del archivo..."
                    ></textarea>
                  </div>
                </div>
              </div>

              <!-- Mensaje de error -->
              <p v-if="error" class="mt-2 text-sm text-red-600">
                {{ error }}
              </p>
            </div>
          </div>
        </div>

        <!-- Botones de acción -->
        <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
          <button
            type="button"
            class="w-full inline-flex justify-center rounded-lg border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:ml-3 sm:w-auto sm:text-sm"
            :disabled="!archivoSeleccionado || subiendo"
            @click="subirArchivo"
          >
            <svg
              v-if="subiendo"
              class="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
            >
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            {{ subiendo ? 'Subiendo...' : 'Subir' }}
          </button>
          <button
            type="button"
            class="mt-3 w-full inline-flex justify-center rounded-lg border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
            @click="$emit('close')"
          >
            Cancelar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useToast } from 'vue-toastification'
import { clienteService } from '../services/clienteService'

const props = defineProps({
  clienteId: {
    type: String,
    required: true
  }
})

const emit = defineEmits(['close', 'uploaded'])

const toast = useToast()
const isDragging = ref(false)
const archivoSeleccionado = ref(null)
const categoria = ref('documentos')
const descripcion = ref('')
const error = ref('')
const subiendo = ref(false)

// Iconos para tipos de archivo
const iconosArchivo = {
  'application/pdf': {
    render() {
      return (
        <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
        </svg>
      )
    }
  },
  'application/vnd.openxmlformats-officedocument.wordprocessingml.document': {
    render() {
      return (
        <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
        </svg>
      )
    }
  },
  'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet': {
    render() {
      return (
        <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h18M3 14h18m-9-4v8m-7 0h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v8a2 2 0 002 2z" />
        </svg>
      )
    }
  },
  'image/': {
    render() {
      return (
        <svg xmlns="http://www.w3.org/2000/svg" class="h-8 w-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
        </svg>
      )
    }
  }
}

function obtenerIconoArchivo(tipo) {
  if (tipo.startsWith('image/')) {
    return iconosArchivo['image/']
  }
  return iconosArchivo[tipo] || iconosArchivo['application/pdf']
}

function formatearTamaño(bytes) {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const sizes = ['Bytes', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i]
}

function validarArchivo(archivo) {
  const tiposPermitidos = [
    'application/pdf',
    'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
    'image/jpeg',
    'image/png'
  ]
  
  const maxSize = 10 * 1024 * 1024 // 10MB
  
  if (!tiposPermitidos.some(tipo => archivo.type.startsWith(tipo))) {
    throw new Error('Tipo de archivo no permitido')
  }
  
  if (archivo.size > maxSize) {
    throw new Error('El archivo excede el tamaño máximo permitido (10MB)')
  }
}

function seleccionarArchivo() {
  const input = document.createElement('input')
  input.type = 'file'
  input.accept = '.pdf,.docx,.xlsx,.jpg,.jpeg,.png'
  input.onchange = (e) => {
    const archivo = e.target.files[0]
    if (archivo) {
      try {
        validarArchivo(archivo)
        archivoSeleccionado.value = archivo
        error.value = ''
      } catch (err) {
        error.value = err.message
      }
    }
  }
  input.click()
}

function onDrop(e) {
  isDragging.value = false
  const archivo = e.dataTransfer.files[0]
  if (archivo) {
    try {
      validarArchivo(archivo)
      archivoSeleccionado.value = archivo
      error.value = ''
    } catch (err) {
      error.value = err.message
    }
  }
}

async function subirArchivo() {
  if (!archivoSeleccionado.value) return
  
  subiendo.value = true
  error.value = ''
  
  try {
    const formData = new FormData()
    formData.append('archivo', archivoSeleccionado.value)
    formData.append('categoria', categoria.value)
    formData.append('descripcion', descripcion.value)
    
    const resultado = await clienteService.subirArchivo(props.clienteId, formData)
    
    toast.success('Archivo subido correctamente')
    emit('uploaded', resultado)
    emit('close')
  } catch (err) {
    console.error('Error al subir archivo:', err)
    error.value = 'Error al subir el archivo. Por favor, intenta nuevamente.'
  } finally {
    subiendo.value = false
  }
}
</script> 