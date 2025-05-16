<template>
  <div class="bg-white rounded-lg shadow">
    <!-- Navegación horizontal -->
    <div class="border-b border-gray-200">
      <nav class="flex space-x-8 px-6" aria-label="Tabs">
        <button
          v-for="fuente in fuentes"
          :key="fuente.id"
          @click="fuenteSeleccionada = fuente.id"
          class="py-4 px-1 border-b-2 font-medium text-sm"
          :class="[
            fuenteSeleccionada === fuente.id
              ? 'border-blue-500 text-blue-600'
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
        >
          {{ fuente.nombre }}
        </button>
      </nav>
    </div>

    <!-- Contenido -->
    <div class="p-6">
      <!-- Skeleton loader -->
      <div v-if="cargando" class="space-y-6">
        <div class="animate-pulse">
          <div class="h-4 bg-gray-200 rounded w-1/4 mb-4"></div>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div v-for="i in 3" :key="i" class="space-y-3">
              <div class="h-4 bg-gray-200 rounded w-3/4"></div>
              <div class="h-10 bg-gray-200 rounded"></div>
            </div>
          </div>
        </div>
      </div>

      <!-- Cards de información -->
      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
          v-for="item in itemsFiltrados"
          :key="item.id"
          class="bg-white rounded-lg border border-gray-200 p-4 hover:shadow-md transition-shadow"
        >
          <div class="flex items-start justify-between">
            <div class="flex items-center space-x-3">
              <div class="flex-shrink-0">
                <component
                  :is="item.icono"
                  class="h-6 w-6 text-gray-400"
                />
              </div>
              <div>
                <h3 class="text-sm font-medium text-gray-900">{{ item.titulo }}</h3>
                <p class="text-xs text-gray-500">{{ item.descripcion }}</p>
              </div>
            </div>
            <span
              class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium"
              :class="[
                item.cumple
                  ? 'bg-green-100 text-green-800'
                  : 'bg-red-100 text-red-800'
              ]"
            >
              {{ item.cumple ? 'Cumple' : 'No cumple' }}
            </span>
          </div>
          
          <div class="mt-4">
            <button
              @click="abrirModal(item)"
              class="text-sm text-blue-600 hover:text-blue-800"
            >
              Ver detalles
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de detalles -->
    <div
      v-if="modalAbierto"
      class="fixed inset-0 z-50 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div
          class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"
          aria-hidden="true"
          @click="cerrarModal"
        ></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div class="mt-3 text-center sm:mt-0 sm:text-left w-full">
                <h3 class="text-lg leading-6 font-medium text-gray-900" id="modal-title">
                  {{ itemSeleccionado.titulo }}
                </h3>
                <div class="mt-4 space-y-4">
                  <div v-for="(valor, key) in itemSeleccionado.detalles" :key="key" class="flex justify-between">
                    <span class="text-sm text-gray-500">{{ key }}</span>
                    <span class="text-sm font-medium text-gray-900">{{ valor }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button
              type="button"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
              @click="cerrarModal"
            >
              Cerrar
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import {
  DocumentTextIcon,
  ChartBarIcon,
  CurrencyDollarIcon,
  DocumentReportIcon
} from '@heroicons/vue/outline'

// Estado
const cargando = ref(true)
const fuenteSeleccionada = ref('veraz')
const modalAbierto = ref(false)
const itemSeleccionado = ref(null)

// Fuentes de información
const fuentes = [
  { id: 'veraz', nombre: 'Veraz' },
  { id: 'nosis', nombre: 'Nosis' },
  { id: 'bcra', nombre: 'BCRA' },
  { id: 'afip', nombre: 'AFIP' }
]

// Datos simulados
const items = [
  {
    id: 1,
    fuente: 'veraz',
    titulo: 'Score Crediticio',
    descripcion: 'Puntaje de riesgo crediticio',
    icono: ChartBarIcon,
    cumple: true,
    detalles: {
      'Score': '750',
      'Categoría': 'Bajo riesgo',
      'Última actualización': '01/03/2024',
      'Tendencia': 'Estable'
    }
  },
  {
    id: 2,
    fuente: 'veraz',
    titulo: 'Deudas Vencidas',
    descripcion: 'Cantidad de deudas vencidas',
    icono: DocumentTextIcon,
    cumple: false,
    detalles: {
      'Cantidad': '2',
      'Monto total': '$150.000',
      'Días de atraso': '45',
      'Entidades': 'Banco A, Financiera B'
    }
  },
  {
    id: 3,
    fuente: 'nosis',
    titulo: 'Riesgo Comercial',
    descripcion: 'Evaluación de riesgo comercial',
    icono: DocumentReportIcon,
    cumple: true,
    detalles: {
      'Nivel de riesgo': 'Bajo',
      'Calificación': 'A',
      'Última actualización': '28/02/2024',
      'Observaciones': 'Sin incidencias'
    }
  },
  {
    id: 4,
    fuente: 'bcra',
    titulo: 'Central de Deudores',
    descripcion: 'Información de deudas bancarias',
    icono: CurrencyDollarIcon,
    cumple: true,
    detalles: {
      'Deudas activas': '3',
      'Monto total': '$500.000',
      'Entidades': 'Banco X, Banco Y, Banco Z',
      'Estado': 'Al día'
    }
  },
  {
    id: 5,
    fuente: 'afip',
    titulo: 'Situación Fiscal',
    descripcion: 'Estado de obligaciones fiscales',
    icono: DocumentTextIcon,
    cumple: true,
    detalles: {
      'Categoría': 'Responsable Inscripto',
      'Estado': 'Al día',
      'Última declaración': 'IVA 02/2024',
      'Observaciones': 'Sin deudas'
    }
  }
]

// Filtrar items por fuente seleccionada
const itemsFiltrados = computed(() => {
  return items.filter(item => item.fuente === fuenteSeleccionada.value)
})

// Funciones
function abrirModal(item) {
  itemSeleccionado.value = item
  modalAbierto.value = true
}

function cerrarModal() {
  modalAbierto.value = false
  itemSeleccionado.value = null
}

// Simular carga de datos
onMounted(() => {
  setTimeout(() => {
    cargando.value = false
  }, 1500)
})
</script> 