<template>
  <div class="space-y-6">
    <!-- Encabezado -->
    <div class="flex items-center justify-between">
      <div>
        <h1 class="text-2xl font-semibold text-gray-900">Análisis Crediticio</h1>
        <p class="mt-1 text-sm text-gray-500">
          Visualización y análisis de métricas crediticias
        </p>
      </div>
      <div class="flex items-center gap-4">
        <select
          v-model="periodoSeleccionado"
          class="rounded-md border-gray-300 py-2 pl-3 pr-10 text-sm focus:border-blue-500 focus:outline-none focus:ring-blue-500"
          :disabled="isLoading"
        >
          <option value="7">Últimos 7 días</option>
          <option value="30">Últimos 30 días</option>
          <option value="90">Últimos 90 días</option>
          <option value="365">Último año</option>
        </select>
        <button
          @click="actualizarDatos"
          class="inline-flex items-center gap-2 rounded-md bg-blue-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600 disabled:opacity-50 disabled:cursor-not-allowed"
          :disabled="isLoading"
        >
          <ArrowPathIcon class="h-4 w-4" :class="{ 'animate-spin': isLoading }" />
          {{ isLoading ? 'Actualizando...' : 'Actualizar' }}
        </button>
      </div>
    </div>

    <!-- Tarjetas de métricas -->
    <div class="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
      <div
        v-for="(metrica, index) in metricas"
        :key="index"
        class="overflow-hidden rounded-lg bg-white shadow"
      >
        <div class="p-5">
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <component
                :is="metrica.icon"
                class="h-6 w-6 text-gray-400"
                aria-hidden="true"
              />
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="truncate text-sm font-medium text-gray-500">
                  {{ metrica.nombre }}
                </dt>
                <dd>
                  <div class="text-lg font-medium text-gray-900">
                    {{ metrica.valor }}
                  </div>
                </dd>
              </dl>
            </div>
          </div>
        </div>
        <div class="bg-gray-50 px-5 py-3">
          <div class="text-sm">
            <span
              :class="[
                metrica.tendencia > 0 ? 'text-green-600' : 'text-red-600',
                'font-medium'
              ]"
            >
              {{ metrica.tendencia > 0 ? '+' : '' }}{{ metrica.tendencia }}%
            </span>
            <span class="text-gray-500"> vs período anterior</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Gráficos -->
    <div class="grid grid-cols-1 gap-6 lg:grid-cols-2">
      <!-- Distribución de Riesgos -->
      <div class="rounded-lg bg-white shadow">
        <div class="p-6">
          <h3 class="text-base font-semibold leading-6 text-gray-900">
            Distribución de Riesgos
          </h3>
          <div class="mt-6">
            <apexchart
              type="donut"
              height="350"
              :options="distribucionRiesgos.chartOptions"
              :series="distribucionRiesgos.series"
            />
          </div>
        </div>
      </div>

      <!-- Tendencias de Crédito -->
      <div class="rounded-lg bg-white shadow">
        <div class="p-6">
          <h3 class="text-base font-semibold leading-6 text-gray-900">
            Tendencias de Crédito
          </h3>
          <div class="mt-6">
            <apexchart
              type="area"
              height="350"
              :options="tendenciasCredito.chartOptions"
              :series="tendenciasCredito.series"
            />
          </div>
        </div>
      </div>
    </div>

    <!-- Tabla de Últimos Análisis -->
    <div class="rounded-lg bg-white shadow">
      <div class="px-4 py-5 sm:px-6">
        <h3 class="text-base font-semibold leading-6 text-gray-900">
          Últimos Análisis Realizados
        </h3>
      </div>
      <div class="border-t border-gray-200">
        <table class="min-w-full divide-y divide-gray-200">
          <thead class="bg-gray-50">
            <tr>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500"
              >
                Cliente
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500"
              >
                Fecha
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500"
              >
                Riesgo
              </th>
              <th
                scope="col"
                class="px-6 py-3 text-left text-xs font-medium uppercase tracking-wider text-gray-500"
              >
                Estado
              </th>
              <th scope="col" class="relative px-6 py-3">
                <span class="sr-only">Acciones</span>
              </th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-200 bg-white">
            <tr v-for="analisis in ultimosAnalisis" :key="analisis.id">
              <td class="whitespace-nowrap px-6 py-4">
                <div class="flex items-center">
                  <div class="h-10 w-10 flex-shrink-0">
                    <div class="h-10 w-10 rounded-full bg-gray-200 flex items-center justify-center">
                      <span class="text-sm font-medium text-gray-600">
                        {{ analisis.cliente.substring(0, 2).toUpperCase() }}
                      </span>
                    </div>
                  </div>
                  <div class="ml-4">
                    <div class="text-sm font-medium text-gray-900">
                      {{ analisis.cliente }}
                    </div>
                  </div>
                </div>
              </td>
              <td class="whitespace-nowrap px-6 py-4">
                <div class="text-sm text-gray-900">{{ analisis.fecha }}</div>
              </td>
              <td class="whitespace-nowrap px-6 py-4">
                <span
                  :class="[
                    'inline-flex rounded-full px-2 text-xs font-semibold leading-5',
                    {
                      'bg-red-100 text-red-800': analisis.riesgo === 'Alto',
                      'bg-yellow-100 text-yellow-800': analisis.riesgo === 'Medio',
                      'bg-green-100 text-green-800': analisis.riesgo === 'Bajo'
                    }
                  ]"
                >
                  {{ analisis.riesgo }}
                </span>
              </td>
              <td class="whitespace-nowrap px-6 py-4">
                <span
                  :class="[
                    'inline-flex rounded-full px-2 text-xs font-semibold leading-5',
                    {
                      'bg-green-100 text-green-800': analisis.estado === 'Aprobado',
                      'bg-red-100 text-red-800': analisis.estado === 'Rechazado',
                      'bg-yellow-100 text-yellow-800': analisis.estado === 'Pendiente'
                    }
                  ]"
                >
                  {{ analisis.estado }}
                </span>
              </td>
              <td class="whitespace-nowrap px-6 py-4 text-right text-sm font-medium">
                <button
                  @click="verDetalle(analisis.id)"
                  class="text-blue-600 hover:text-blue-900"
                >
                  Ver detalle
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import VueApexCharts from 'vue3-apexcharts'
import { useToast } from 'vue-toastification'
import {
  ArrowPathIcon,
  ChartBarIcon,
  UserGroupIcon,
  CurrencyDollarIcon,
  ExclamationTriangleIcon
} from '@heroicons/vue/24/outline'
import analisisService from '../services/analisisService'

const toast = useToast()
const isLoading = ref(false)

// Estado
const periodoSeleccionado = ref('30')

// Configuración del gráfico de distribución de riesgos
const distribucionRiesgos = ref({
  series: [],
  chartOptions: {
    chart: {
      type: 'donut',
      height: 350
    },
    labels: ['Bajo', 'Medio', 'Alto'],
    colors: ['#10B981', '#F59E0B', '#EF4444'],
    legend: {
      position: 'bottom'
    },
    responsive: [{
      breakpoint: 480,
      options: {
        chart: {
          width: 200
        },
        legend: {
          position: 'bottom'
        }
      }
    }]
  }
})

// Configuración del gráfico de tendencias
const tendenciasCredito = ref({
  series: [{
    name: 'Monto Analizado',
    data: []
  }],
  chartOptions: {
    chart: {
      type: 'area',
      height: 350,
      toolbar: {
        show: false
      }
    },
    dataLabels: {
      enabled: false
    },
    stroke: {
      curve: 'smooth',
      width: 2
    },
    xaxis: {
      categories: [],
      labels: {
        style: {
          colors: '#6B7280'
        }
      }
    },
    yaxis: {
      labels: {
        style: {
          colors: '#6B7280'
        },
        formatter: function(value) {
          return '$' + value + 'M'
        }
      }
    },
    tooltip: {
      y: {
        formatter: function(value) {
          return '$' + value + 'M'
        }
      }
    },
    fill: {
      type: 'gradient',
      gradient: {
        shadeIntensity: 1,
        opacityFrom: 0.7,
        opacityTo: 0.9,
        stops: [0, 90, 100]
      }
    },
    colors: ['#3B82F6']
  }
})

// Métricas
const metricas = ref([
  {
    nombre: 'Total de Análisis',
    valor: '0',
    tendencia: 0,
    icon: ChartBarIcon
  },
  {
    nombre: 'Clientes Activos',
    valor: '0',
    tendencia: 0,
    icon: UserGroupIcon
  },
  {
    nombre: 'Monto Total Analizado',
    valor: '$0',
    tendencia: 0,
    icon: CurrencyDollarIcon
  },
  {
    nombre: 'Alertas Activas',
    valor: '0',
    tendencia: 0,
    icon: ExclamationTriangleIcon
  }
])

// Últimos análisis
const ultimosAnalisis = ref([])

// Cargar datos
const cargarDatos = async () => {
  isLoading.value = true
  try {
    // Cargar métricas
    const metricasData = await analisisService.getMetricas(periodoSeleccionado.value)
    metricas.value = metricasData

    // Cargar distribución de riesgos
    const distribucionData = await analisisService.getDistribucionRiesgos(periodoSeleccionado.value)
    distribucionRiesgos.value.series = distribucionData.series

    // Cargar tendencias
    const tendenciasData = await analisisService.getTendenciasCredito(periodoSeleccionado.value)
    tendenciasCredito.value.series[0].data = tendenciasData.valores
    tendenciasCredito.value.chartOptions.xaxis.categories = tendenciasData.categorias

    // Cargar últimos análisis
    const ultimosData = await analisisService.getUltimosAnalisis()
    ultimosAnalisis.value = ultimosData

    toast.success('Datos actualizados correctamente')
  } catch (error) {
    console.error('Error al cargar datos:', error)
    toast.error('Error al cargar los datos. Por favor, intente nuevamente.')
  } finally {
    isLoading.value = false
  }
}

// Observar cambios en el período
watch(periodoSeleccionado, () => {
  cargarDatos()
})

// Cargar datos al montar el componente
onMounted(() => {
  cargarDatos()
})

// Métodos
const actualizarDatos = () => {
  cargarDatos()
}

const verDetalle = (id) => {
  // Aquí iría la lógica para ver el detalle del análisis
  console.log('Ver detalle del análisis:', id)
}
</script> 