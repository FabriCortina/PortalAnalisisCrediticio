<template>
  <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
    <!-- Encabezado -->
    <div class="mb-8">
      <h1 class="text-2xl font-bold text-gray-900">Dashboard</h1>
      <p class="mt-1 text-sm text-gray-500">
        Resumen general del sistema
      </p>
    </div>

    <!-- KPIs -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      <!-- Total Clientes -->
      <div class="bg-white overflow-hidden shadow rounded-lg">
        <div class="p-5">
          <div class="flex items-center">
            <div class="flex-shrink-0 bg-blue-500 rounded-md p-3">
              <UserGroupIcon class="h-6 w-6 text-white" />
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="text-sm font-medium text-gray-500 truncate">
                  Total Clientes
                </dt>
                <dd class="flex items-baseline">
                  <div class="text-2xl font-semibold text-gray-900">
                    {{ kpis.totalClientes }}
                  </div>
                </dd>
              </dl>
            </div>
          </div>
        </div>
      </div>

      <!-- Créditos Activos -->
      <div class="bg-white overflow-hidden shadow rounded-lg">
        <div class="p-5">
          <div class="flex items-center">
            <div class="flex-shrink-0 bg-green-500 rounded-md p-3">
              <CreditCardIcon class="h-6 w-6 text-white" />
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="text-sm font-medium text-gray-500 truncate">
                  Créditos Activos
                </dt>
                <dd class="flex items-baseline">
                  <div class="text-2xl font-semibold text-gray-900">
                    {{ kpis.creditosActivos }}
                  </div>
                </dd>
              </dl>
            </div>
          </div>
        </div>
      </div>

      <!-- Monto Total -->
      <div class="bg-white overflow-hidden shadow rounded-lg">
        <div class="p-5">
          <div class="flex items-center">
            <div class="flex-shrink-0 bg-purple-500 rounded-md p-3">
              <CurrencyDollarIcon class="h-6 w-6 text-white" />
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="text-sm font-medium text-gray-500 truncate">
                  Monto Total
                </dt>
                <dd class="flex items-baseline">
                  <div class="text-2xl font-semibold text-gray-900">
                    ${{ kpis.montoTotal.toLocaleString('es-AR') }}
                  </div>
                </dd>
              </dl>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Gráficos -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Distribución de Riesgos -->
      <div class="bg-white shadow rounded-lg p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">
          Distribución de Riesgos
        </h3>
        <div class="h-80">
          <Pie
            v-if="distribucionRiesgos"
            :data="distribucionRiesgos"
            :options="opcionesPie"
          />
        </div>
      </div>

      <!-- Solicitudes por Mes -->
      <div class="bg-white shadow rounded-lg p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">
          Solicitudes por Mes
        </h3>
        <div class="h-80">
          <Bar
            v-if="solicitudesPorMes"
            :data="solicitudesPorMes"
            :options="opcionesBar"
          />
        </div>
      </div>
    </div>

    <!-- Estado de carga -->
    <div v-if="loading" class="flex justify-center items-center py-8">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
    </div>

    <!-- Mensaje de error -->
    <div v-else-if="error" class="bg-red-50 border-l-4 border-red-400 p-4 mt-6">
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
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { dashboardService } from '@/services/dashboardService'
import { useToast } from '@/composables/useToast'
import { UserGroupIcon, CreditCardIcon, CurrencyDollarIcon } from '@heroicons/vue/outline'
import { Chart as ChartJS, ArcElement, Tooltip, Legend, CategoryScale, LinearScale, BarElement, Title } from 'chart.js'
import { Pie, Bar } from 'vue-chartjs'

ChartJS.register(ArcElement, Tooltip, Legend, CategoryScale, LinearScale, BarElement, Title)

const { showToast } = useToast()

// Estado
const loading = ref(false)
const error = ref(null)
const kpis = ref({
  totalClientes: 0,
  creditosActivos: 0,
  montoTotal: 0
})
const distribucionRiesgos = ref(null)
const solicitudesPorMes = ref(null)

// Opciones de gráficos
const opcionesPie = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'bottom'
    }
  }
}

const opcionesBar = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      display: false
    }
  },
  scales: {
    y: {
      beginAtZero: true,
      ticks: {
        stepSize: 1
      }
    }
  }
}

// Cargar datos
async function cargarDatos() {
  loading.value = true
  error.value = null

  try {
    // Cargar KPIs
    const kpisData = await dashboardService.getKPIs()
    kpis.value = kpisData

    // Cargar distribución de riesgos
    const riesgosData = await dashboardService.getDistribucionRiesgos()
    distribucionRiesgos.value = {
      labels: ['Alto', 'Medio', 'Bajo'],
      datasets: [{
        data: [
          riesgosData.alto,
          riesgosData.medio,
          riesgosData.bajo
        ],
        backgroundColor: [
          '#EF4444', // Rojo
          '#F59E0B', // Amarillo
          '#10B981'  // Verde
        ]
      }]
    }

    // Cargar solicitudes por mes
    const solicitudesData = await dashboardService.getSolicitudesPorMes()
    solicitudesPorMes.value = {
      labels: solicitudesData.map(item => item.mes),
      datasets: [{
        label: 'Solicitudes',
        data: solicitudesData.map(item => item.cantidad),
        backgroundColor: '#3B82F6' // Azul
      }]
    }
  } catch (err) {
    error.value = 'Error al cargar los datos del dashboard'
    showToast('Error al cargar los datos del dashboard', 'error')
  } finally {
    loading.value = false
  }
}

// Cargar datos al montar
onMounted(cargarDatos)
</script> 