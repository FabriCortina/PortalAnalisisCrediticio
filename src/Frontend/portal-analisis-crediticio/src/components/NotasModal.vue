<template>
  <div class="fixed inset-0 z-50 overflow-y-auto">
    <div class="flex items-center justify-center min-h-screen px-4 pt-4 pb-20 text-center sm:block sm:p-0">
      <!-- Fondo oscuro -->
      <div class="fixed inset-0 transition-opacity" aria-hidden="true">
        <div class="absolute inset-0 bg-gray-500 opacity-75"></div>
      </div>

      <!-- Modal -->
      <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-4xl sm:w-full">
        <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
          <div class="sm:flex sm:items-start">
            <div class="mt-3 text-center sm:mt-0 sm:text-left w-full">
              <div class="flex justify-between items-center mb-4">
                <h3 class="text-lg leading-6 font-medium text-gray-900">
                  Notas - {{ archivo.nombre }}
                </h3>
                <div class="flex items-center space-x-2">
                  <button
                    type="button"
                    class="text-gray-400 hover:text-gray-500"
                    @click="toggleVista"
                  >
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                    </svg>
                  </button>
                </div>
              </div>

              <!-- Editor y Vista Previa -->
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <!-- Editor -->
                <div class="space-y-2">
                  <div class="flex items-center justify-between">
                    <label class="block text-sm font-medium text-gray-700">
                      Editor
                    </label>
                    <div class="flex items-center space-x-2">
                      <button
                        v-for="accion in accionesMarkdown"
                        :key="accion.nombre"
                        type="button"
                        class="p-1 text-gray-400 hover:text-gray-600"
                        :title="accion.descripcion"
                        @click="insertarMarkdown(accion.accion)"
                      >
                        <component :is="accion.icono" class="h-4 w-4" />
                      </button>
                    </div>
                  </div>
                  <textarea
                    v-model="contenido"
                    rows="15"
                    class="w-full rounded-lg border-gray-300 focus:ring-blue-500 focus:border-blue-500 font-mono text-sm"
                    placeholder="Escribe tus notas aquí..."
                  ></textarea>
                </div>

                <!-- Vista Previa -->
                <div class="space-y-2">
                  <label class="block text-sm font-medium text-gray-700">
                    Vista Previa
                  </label>
                  <div
                    class="w-full rounded-lg border border-gray-300 p-4 min-h-[300px] prose prose-sm max-w-none"
                    v-html="vistaPrevia"
                  ></div>
                </div>
              </div>

              <!-- Historial de cambios -->
              <div class="mt-6">
                <h4 class="text-sm font-medium text-gray-700 mb-2">
                  Historial de cambios
                </h4>
                <div class="space-y-2">
                  <div
                    v-for="cambio in historial"
                    :key="cambio.id"
                    class="flex items-start space-x-3 text-sm"
                  >
                    <div class="flex-shrink-0">
                      <div class="h-8 w-8 rounded-full bg-gray-200 flex items-center justify-center">
                        <span class="text-gray-600 font-medium">
                          {{ cambio.usuario.charAt(0).toUpperCase() }}
                        </span>
                      </div>
                    </div>
                    <div class="flex-1">
                      <p class="text-gray-900">
                        {{ cambio.usuario }}
                        <span class="text-gray-500">
                          {{ cambio.accion }}
                        </span>
                      </p>
                      <p class="text-gray-500 text-xs">
                        {{ formatearFecha(cambio.fecha) }}
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Botones de acción -->
        <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
          <button
            type="button"
            class="w-full inline-flex justify-center rounded-lg border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:ml-3 sm:w-auto sm:text-sm"
            :disabled="guardando"
            @click="guardarNotas"
          >
            <svg
              v-if="guardando"
              class="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
            >
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            {{ guardando ? 'Guardando...' : 'Guardar' }}
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
import { ref, computed, onMounted } from 'vue'
import { useToast } from 'vue-toastification'
import { marked } from 'marked'
import { clienteService } from '../services/clienteService'
import DOMPurify from 'dompurify'

const props = defineProps({
  archivo: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['close', 'guardar'])

const toast = useToast()
const contenido = ref('')
const guardando = ref(false)
const historial = ref([])

// Acciones de markdown
const accionesMarkdown = [
  {
    nombre: 'negrita',
    descripcion: 'Texto en negrita',
    icono: {
      render() {
        return (
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 12h8a4 4 0 100-8H6v8zm0 0h8a4 4 0 110 8H6v-8z" />
          </svg>
        )
      }
    },
    accion: () => insertarFormato('**', '**')
  },
  {
    nombre: 'cursiva',
    descripcion: 'Texto en cursiva',
    icono: {
      render() {
        return (
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 20l4-16m4 4l-4 4-4-4" />
          </svg>
        )
      }
    },
    accion: () => insertarFormato('*', '*')
  },
  {
    nombre: 'lista',
    descripcion: 'Lista con viñetas',
    icono: {
      render() {
        return (
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        )
      }
    },
    accion: () => insertarFormato('- ', '')
  },
  {
    nombre: 'enlace',
    descripcion: 'Insertar enlace',
    icono: {
      render() {
        return (
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.828 10.172a4 4 0 00-5.656 0l-4 4a4 4 0 105.656 5.656l1.102-1.101m-.758-4.899a4 4 0 005.656 0l4-4a4 4 0 00-5.656-5.656l-1.1 1.1" />
          </svg>
        )
      }
    },
    accion: () => insertarFormato('[', '](url)')
  }
]

// Vista previa del markdown
const vistaPrevia = computed(() => {
  return DOMPurify.sanitize(marked(contenido.value))
})

// Funciones
function insertarFormato(prefijo, sufijo) {
  const textarea = document.querySelector('textarea')
  const start = textarea.selectionStart
  const end = textarea.selectionEnd
  const texto = contenido.value
  const seleccion = texto.substring(start, end)
  
  contenido.value = texto.substring(0, start) + prefijo + seleccion + sufijo + texto.substring(end)
  
  // Restaurar el foco y la selección
  setTimeout(() => {
    textarea.focus()
    textarea.setSelectionRange(
      start + prefijo.length,
      end + prefijo.length
    )
  }, 0)
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

async function guardarNotas() {
  guardando.value = true
  
  try {
    const resultado = await clienteService.guardarNotas(props.archivo.id, {
      contenido: contenido.value
    })
    
    toast.success('Notas guardadas correctamente')
    emit('guardar', resultado)
    emit('close')
  } catch (err) {
    console.error('Error al guardar notas:', err)
    toast.error('Error al guardar las notas')
  } finally {
    guardando.value = false
  }
}

// Cargar datos
async function cargarDatos() {
  try {
    const [notas, historialData] = await Promise.all([
      clienteService.getNotas(props.archivo.id),
      clienteService.getHistorialNotas(props.archivo.id)
    ])
    
    contenido.value = notas.contenido
    historial.value = historialData
  } catch (err) {
    console.error('Error al cargar datos:', err)
    toast.error('Error al cargar las notas')
  }
}

onMounted(cargarDatos)
</script>

<style>
.prose {
  max-width: none;
}
.prose pre {
  background-color: #f3f4f6;
  padding: 1rem;
  border-radius: 0.5rem;
  overflow-x: auto;
}
.prose code {
  background-color: #f3f4f6;
  padding: 0.2rem 0.4rem;
  border-radius: 0.25rem;
  font-size: 0.875em;
}
.prose blockquote {
  border-left: 4px solid #e5e7eb;
  padding-left: 1rem;
  font-style: italic;
  color: #4b5563;
}
</style> 