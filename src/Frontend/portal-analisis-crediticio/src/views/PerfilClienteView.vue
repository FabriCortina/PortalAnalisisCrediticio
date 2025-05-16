<template>
  <div class="container mx-auto px-4 py-8">
    <!-- Encabezado -->
    <div class="bg-white rounded-xl shadow-sm p-6 mb-6">
      <div class="flex justify-between items-start">
        <div>
          <h1 class="text-2xl font-bold text-gray-900">{{ cliente.nombre }}</h1>
          <p class="text-gray-600">CUIT: {{ cliente.cuit }}</p>
        </div>
        <div class="flex space-x-4">
          <button
            class="inline-flex items-center px-4 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200"
            @click="volver"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
            Volver
          </button>
        </div>
      </div>
    </div>

    <!-- Navegación por pestañas -->
    <div class="bg-white rounded-xl shadow-sm mb-6">
      <nav class="flex border-b border-gray-200">
        <button
          v-for="tab in tabs"
          :key="tab.id"
          class="px-6 py-4 text-sm font-medium border-b-2 -mb-px"
          :class="[
            tab.id === tabActiva
              ? 'border-blue-500 text-blue-600'
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
          ]"
          @click="tabActiva = tab.id"
        >
          <div class="flex items-center">
            <component :is="tab.icon" class="h-5 w-5 mr-2" />
            {{ tab.nombre }}
          </div>
        </button>
      </nav>
    </div>

    <!-- Contenido de las pestañas -->
    <div class="bg-white rounded-xl shadow-sm p-6">
      <!-- Skeleton loader -->
      <div v-if="cargando" class="space-y-6">
        <div class="animate-pulse">
          <div class="h-4 bg-gray-200 rounded w-1/4 mb-4"></div>
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div v-for="i in 6" :key="i" class="space-y-3">
              <div class="h-4 bg-gray-200 rounded w-3/4"></div>
              <div class="h-10 bg-gray-200 rounded"></div>
            </div>
          </div>
        </div>
      </div>

      <!-- Contenido dinámico -->
      <template v-else>
        <FormularioFinanciero
          v-if="tabActiva === 'balance'"
          tipo="balance"
          :datos-iniciales="datosFinancieros.balance"
          @submit="guardarDatos"
          @cancel="volver"
        />
        <FormularioFinanciero
          v-else-if="tabActiva === 'resultados'"
          tipo="resultados"
          :datos-iniciales="datosFinancieros.resultados"
          @submit="guardarDatos"
          @cancel="volver"
        />
        <FormularioFinanciero
          v-else-if="tabActiva === 'cashflow'"
          tipo="cashflow"
          :datos-iniciales="datosFinancieros.cashflow"
          @submit="guardarDatos"
          @cancel="volver"
        />
        <FormularioFinanciero
          v-else-if="tabActiva === 'deudas'"
          tipo="deudas"
          :datos-iniciales="datosFinancieros.deudas"
          @submit="guardarDatos"
          @cancel="volver"
        />
      </template>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useToast } from 'vue-toastification'
import FormularioFinanciero from '../components/FormularioFinanciero.vue'
import { clienteService } from '../services/clienteService'

// Iconos para las pestañas
const BalanceIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 6l3 1m0 0l-3 9a5.002 5.002 0 006.001 0M6 7l3 9M6 7l6-2m6 2l3-1m-3 1l-3 9a5.002 5.002 0 006.001 0M18 7l3 9m-3-9l-6-2m0-2v2m0 16V5m0 16H9m3 0h3" />
      </svg>
    )
  }
}

const ResultadosIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
      </svg>
    )
  }
}

const CashFlowIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
    )
  }
}

const DeudasIcon = {
  render() {
    return (
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
    )
  }
}

const route = useRoute()
const router = useRouter()
const toast = useToast()

// Estado
const cliente = ref({})
const cargando = ref(true)
const tabActiva = ref('balance')
const datosFinancieros = ref({
  balance: {},
  resultados: {},
  cashflow: {},
  deudas: {}
})

// Configuración de pestañas
const tabs = [
  { id: 'balance', nombre: 'Balance', icon: BalanceIcon },
  { id: 'resultados', nombre: 'Resultados', icon: ResultadosIcon },
  { id: 'cashflow', nombre: 'Cash Flow', icon: CashFlowIcon },
  { id: 'deudas', nombre: 'Deudas', icon: DeudasIcon }
]

// Cargar datos del cliente
async function cargarDatos() {
  try {
    cargando.value = true
    const clienteId = route.params.id
    const [clienteData, infoFinanciera] = await Promise.all([
      clienteService.getCliente(clienteId),
      clienteService.getInformacionFinanciera(clienteId)
    ])
    
    cliente.value = clienteData
    datosFinancieros.value = infoFinanciera
  } catch (error) {
    console.error('Error al cargar datos:', error)
    toast.error('Error al cargar los datos del cliente')
  } finally {
    cargando.value = false
  }
}

// Guardar datos
async function guardarDatos(datos) {
  try {
    const clienteId = route.params.id
    await clienteService.actualizarInformacionFinanciera(clienteId, {
      tipo: tabActiva.value,
      datos
    })
    toast.success('Datos guardados correctamente')
    await cargarDatos() // Recargar datos actualizados
  } catch (error) {
    console.error('Error al guardar datos:', error)
    toast.error('Error al guardar los datos')
  }
}

// Navegación
function volver() {
  router.push('/clientes')
}

onMounted(cargarDatos)
</script>

<style scoped>
.animate-pulse {
  animation: pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

@keyframes pulse {
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: .5;
  }
}
</style> 