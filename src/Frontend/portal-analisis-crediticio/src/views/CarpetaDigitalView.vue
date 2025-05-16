<template>
  <div class="container mx-auto px-4 py-8">
    <!-- Encabezado con búsqueda -->
    <div class="bg-white rounded-xl shadow-sm p-6 mb-6">
      <div class="flex justify-between items-center">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">Carpeta Digital</h1>
          <p class="text-gray-600">{{ cliente.nombre }} - CUIT: {{ cliente.cuit }}</p>
        </div>
        <div class="flex items-center space-x-4">
          <!-- Búsqueda global -->
          <div class="relative">
            <input
              type="text"
              v-model="busqueda"
              placeholder="Buscar en la carpeta..."
              class="w-64 pl-10 pr-4 py-2 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            />
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-400 absolute left-3 top-2.5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </div>
          <button
            class="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
            @click="mostrarUploadModal = true"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12" />
            </svg>
            Subir Archivo
          </button>
        </div>
      </div>
    </div>

    <!-- Accesos rápidos -->
    <div class="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
      <button
        v-for="acceso in accesosRapidos"
        :key="acceso.id"
        class="bg-white rounded-xl shadow-sm p-4 hover:shadow-md transition-shadow"
        @click="navegarA(acceso.ruta)"
      >
        <div class="flex items-center space-x-3">
          <component :is="acceso.icono" class="h-6 w-6 text-blue-600" />
          <div class="text-left">
            <h3 class="font-medium text-gray-900">{{ acceso.titulo }}</h3>
            <p class="text-sm text-gray-500">{{ acceso.descripcion }}</p>
          </div>
        </div>
      </button>
    </div>

    <!-- Contenido principal -->
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <!-- Panel izquierdo: Categorías -->
      <div class="lg:col-span-1">
        <div class="bg-white rounded-xl shadow-sm p-6">
          <h2 class="text-lg font-semibold text-gray-900 mb-4">Categorías</h2>
          <nav class="space-y-2">
            <button
              v-for="categoria in categorias"
              :key="categoria.id"
              class="w-full flex items-center justify-between px-4 py-2 text-sm rounded-lg"
              :class="[
                categoriaSeleccionada === categoria.id
                  ? 'bg-blue-50 text-blue-700'
                  : 'text-gray-700 hover:bg-gray-50'
              ]"
              @click="categoriaSeleccionada = categoria.id"
            >
              <div class="flex items-center">
                <component :is="categoria.icono" class="h-5 w-5 mr-3" />
                {{ categoria.nombre }}
              </div>
              <span class="bg-gray-100 text-gray-600 px-2 py-1 rounded-full text-xs">
                {{ categoria.cantidad }}
              </span>
            </button>
          </nav>
        </div>
      </div>

      <!-- Panel central: Lista de archivos -->
      <div class="lg:col-span-2">
        <div class="bg-white rounded-xl shadow-sm p-6">
          <div class="flex justify-between items-center mb-6">
            <h2 class="text-lg font-semibold text-gray-900">
              {{ categorias.find(c => c.id === categoriaSeleccionada)?.nombre }}
            </h2>
            <div class="flex items-center space-x-4">
              <select
                v-model="ordenamiento"
                class="rounded-lg border-gray-300 text-sm focus:ring-blue-500 focus:border-blue-500"
              >
                <option value="fecha_desc">Más recientes</option>
                <option value="fecha_asc">Más antiguos</option>
                <option value="nombre_asc">Nombre A-Z</option>
                <option value="nombre_desc">Nombre Z-A</option>
              </select>
            </div>
          </div>

          <!-- Lista de archivos -->
          <div class="space-y-4">
            <div
              v-for="archivo in archivosFiltrados"
              :key="archivo.id"
              class="flex items-center justify-between p-4 bg-gray-50 rounded-lg hover:bg-gray-100"
            >
              <div class="flex items-center space-x-4">
                <component :is="obtenerIconoArchivo(archivo.tipo)" class="h-8 w-8 text-gray-500" />
                <div>
                  <h3 class="font-medium text-gray-900">{{ archivo.nombre }}</h3>
                  <p class="text-sm text-gray-500">
                    {{ archivo.fecha }} · {{ archivo.tamaño }}
                  </p>
                </div>
              </div>
              <div class="flex items-center space-x-2">
                <button
                  class="p-2 text-gray-400 hover:text-gray-600"
                  @click="previsualizarArchivo(archivo)"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                  </svg>
                </button>
                <button
                  class="p-2 text-gray-400 hover:text-gray-600"
                  @click="descargarArchivo(archivo)"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                  </svg>
                </button>
                <button
                  class="p-2 text-gray-400 hover:text-gray-600"
                  @click="mostrarNotas(archivo)"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                  </svg>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de subida de archivos -->
    <UploadModal
      v-if="mostrarUploadModal"
      @close="mostrarUploadModal = false"
      @uploaded="onArchivoSubido"
    />

    <!-- Modal de notas -->
    <NotasModal
      v-if="mostrarNotasModal"
      :archivo="archivoSeleccionado"
      @close="mostrarNotasModal = false"
      @guardar="guardarNotas"
    />

    <!-- Modal de previsualización -->
    <PreviewModal
      v-if="mostrarPreviewModal"
      :archivo="archivoSeleccionado"
      @close="mostrarPreviewModal = false"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useToast } from 'vue-toastification'
import UploadModal from '../components/UploadModal.vue'
import NotasModal from '../components/NotasModal.vue'
import PreviewModal from '../components/PreviewModal.vue'
import { clienteService } from '../services/clienteService'

// Iconos para accesos rápidos
const AnalisisIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
      </svg>
    )
  }
}

const HistorialIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
    )
  }
}

const EdicionIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
      </svg>
    )
  }
}

const DocumentosIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
      </svg>
    )
  }
}

// Estado
const route = useRoute()
const router = useRouter()
const toast = useToast()

const cliente = ref({})
const busqueda = ref('')
const categoriaSeleccionada = ref('documentos')
const ordenamiento = ref('fecha_desc')
const mostrarUploadModal = ref(false)
const mostrarNotasModal = ref(false)
const mostrarPreviewModal = ref(false)
const archivoSeleccionado = ref(null)

// Accesos rápidos
const accesosRapidos = [
  {
    id: 'analisis',
    titulo: 'Análisis',
    descripcion: 'Ver análisis crediticio',
    icono: AnalisisIcon,
    ruta: '/analisis'
  },
  {
    id: 'historial',
    titulo: 'Historial',
    descripcion: 'Ver historial de cambios',
    icono: HistorialIcon,
    ruta: '/historial'
  },
  {
    id: 'edicion',
    titulo: 'Edición',
    descripcion: 'Editar información',
    icono: EdicionIcon,
    ruta: '/edicion'
  },
  {
    id: 'documentos',
    titulo: 'Documentos',
    descripcion: 'Gestionar documentos',
    icono: DocumentosIcon,
    ruta: '/documentos'
  }
]

// Categorías
const categorias = [
  {
    id: 'documentos',
    nombre: 'Documentos',
    icono: DocumentosIcon,
    cantidad: 0
  },
  {
    id: 'entrevistas',
    nombre: 'Entrevistas',
    icono: HistorialIcon,
    cantidad: 0
  },
  {
    id: 'garantias',
    nombre: 'Garantías',
    icono: EdicionIcon,
    cantidad: 0
  },
  {
    id: 'analisis',
    nombre: 'Análisis',
    icono: AnalisisIcon,
    cantidad: 0
  }
]

// Archivos filtrados
const archivosFiltrados = computed(() => {
  let archivos = [...archivos.value]
  
  // Filtrar por categoría
  archivos = archivos.filter(a => a.categoria === categoriaSeleccionada.value)
  
  // Filtrar por búsqueda
  if (busqueda.value) {
    const busquedaLower = busqueda.value.toLowerCase()
    archivos = archivos.filter(a => 
      a.nombre.toLowerCase().includes(busquedaLower) ||
      a.descripcion?.toLowerCase().includes(busquedaLower)
    )
  }
  
  // Ordenar
  archivos.sort((a, b) => {
    switch (ordenamiento.value) {
      case 'fecha_desc':
        return new Date(b.fecha) - new Date(a.fecha)
      case 'fecha_asc':
        return new Date(a.fecha) - new Date(b.fecha)
      case 'nombre_asc':
        return a.nombre.localeCompare(b.nombre)
      case 'nombre_desc':
        return b.nombre.localeCompare(a.nombre)
      default:
        return 0
    }
  })
  
  return archivos
})

// Funciones
function navegarA(ruta) {
  router.push(ruta)
}

function obtenerIconoArchivo(tipo) {
  // Implementar lógica para devolver el icono según el tipo de archivo
  return DocumentosIcon
}

function previsualizarArchivo(archivo) {
  archivoSeleccionado.value = archivo
  mostrarPreviewModal.value = true
}

function descargarArchivo(archivo) {
  // Implementar lógica de descarga
}

function mostrarNotas(archivo) {
  archivoSeleccionado.value = archivo
  mostrarNotasModal.value = true
}

function guardarNotas(notas) {
  // Implementar lógica para guardar notas
}

function onArchivoSubido(archivo) {
  // Implementar lógica después de subir archivo
}

// Cargar datos
async function cargarDatos() {
  try {
    const clienteId = route.params.id
    const [clienteData, archivosData] = await Promise.all([
      clienteService.getCliente(clienteId),
      clienteService.getArchivos(clienteId)
    ])
    
    cliente.value = clienteData
    archivos.value = archivosData
    
    // Actualizar contadores de categorías
    categorias.forEach(cat => {
      cat.cantidad = archivosData.filter(a => a.categoria === cat.id).length
    })
  } catch (error) {
    console.error('Error al cargar datos:', error)
    toast.error('Error al cargar los datos de la carpeta')
  }
}

onMounted(cargarDatos)
</script> 