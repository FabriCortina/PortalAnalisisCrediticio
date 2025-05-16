<template>
  <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
    <!-- Encabezado -->
    <div class="mb-8 flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Informes</h1>
        <p class="mt-1 text-sm text-gray-500">
          Informes y reportes del sistema
        </p>
      </div>
      <div class="flex space-x-4">
        <ExportButton
          :columnas="columnasExportacion"
          :datos="informes"
          nombre-archivo="informes"
        />
      </div>
    </div>

    <!-- Lista de informes -->
    <div class="bg-white shadow overflow-hidden sm:rounded-md">
      <ul role="list" class="divide-y divide-gray-200">
        <li v-for="informe in informes" :key="informe.id">
          <div class="px-4 py-4 sm:px-6">
            <!-- Encabezado del informe -->
            <div class="flex items-center justify-between">
              <div class="flex items-center">
                <div class="flex-shrink-0">
                  <component
                    :is="informe.icono"
                    class="h-6 w-6 text-gray-400"
                  />
                </div>
                <div class="ml-4">
                  <h3 class="text-lg font-medium text-gray-900">
                    {{ informe.tipo }}
                  </h3>
                  <p class="text-sm text-gray-500">
                    Generado por {{ informe.autor }}
                  </p>
                </div>
              </div>
              <div class="flex items-center space-x-4">
                <span class="text-sm text-gray-500">
                  {{ formatoFecha(informe.fecha) }}
                </span>
                <button
                  @click="toggleDetalle(informe.id)"
                  class="text-gray-400 hover:text-gray-500"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="h-5 w-5"
                    :class="{ 'transform rotate-180': informe.mostrarDetalle }"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                  >
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
                  </svg>
                </button>
              </div>
            </div>

            <!-- Detalle del informe -->
            <div
              v-if="informe.mostrarDetalle"
              class="mt-4 border-t border-gray-200 pt-4"
            >
              <div class="prose max-w-none">
                <h4 class="text-sm font-medium text-gray-900 mb-2">Resumen</h4>
                <p class="text-sm text-gray-700 mb-4">{{ informe.resumen }}</p>

                <h4 class="text-sm font-medium text-gray-900 mb-2">Hallazgos Principales</h4>
                <ul class="list-disc pl-5 text-sm text-gray-700 space-y-2">
                  <li v-for="(hallazgo, index) in informe.hallazgos" :key="index">
                    {{ hallazgo }}
                  </li>
                </ul>

                <div class="mt-4 flex flex-wrap gap-2">
                  <span
                    v-for="(tag, index) in informe.tags"
                    :key="index"
                    class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium"
                    :class="{
                      'bg-blue-100 text-blue-800': tag.tipo === 'info',
                      'bg-yellow-100 text-yellow-800': tag.tipo === 'warning',
                      'bg-green-100 text-green-800': tag.tipo === 'success'
                    }"
                  >
                    {{ tag.texto }}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import {
  DocumentTextIcon,
  ChartBarIcon,
  DocumentReportIcon,
  ClipboardCheckIcon
} from '@heroicons/vue/outline'
import ExportButton from '@/components/ExportButton.vue'
import { informeService } from '@/services/informeService'
import { useToast } from '@/composables/useToast'

// Estado
const informes = ref([
  {
    id: 1,
    tipo: 'Análisis de Riesgo',
    autor: 'Juan Pérez',
    fecha: '2024-03-15T10:30:00',
    icono: ChartBarIcon,
    mostrarDetalle: false,
    resumen: 'Evaluación completa del riesgo crediticio basada en múltiples fuentes de información.',
    hallazgos: [
      'Score crediticio: 750 puntos',
      'Sin deudas vencidas en los últimos 12 meses',
      'Antigüedad laboral: 3 años',
      'Ingresos estables y verificables'
    ],
    tags: [
      { texto: 'Riesgo Medio', tipo: 'warning' },
      { texto: 'Veraz', tipo: 'info' },
      { texto: 'BCRA', tipo: 'info' }
    ]
  },
  {
    id: 2,
    tipo: 'Evaluación Financiera',
    autor: 'María González',
    fecha: '2024-03-10T15:45:00',
    icono: DocumentReportIcon,
    mostrarDetalle: false,
    resumen: 'Análisis detallado de la situación financiera y capacidad de pago.',
    hallazgos: [
      'Capacidad de pago: 35% del ingreso mensual',
      'Deuda total: $500.000',
      'Ratio de endeudamiento: 0.45',
      'Ahorro mensual promedio: $50.000'
    ],
    tags: [
      { texto: 'Buen perfil', tipo: 'success' },
      { texto: 'Financiero', tipo: 'info' }
    ]
  },
  {
    id: 3,
    tipo: 'Verificación de Documentos',
    autor: 'Carlos Rodríguez',
    fecha: '2024-03-05T09:15:00',
    icono: ClipboardCheckIcon,
    mostrarDetalle: false,
    resumen: 'Revisión y validación de documentación presentada.',
    hallazgos: [
      'DNI verificado y vigente',
      'Comprobante de ingresos validado',
      'Domicilio confirmado',
      'Sin observaciones en documentación'
    ],
    tags: [
      { texto: 'Completo', tipo: 'success' },
      { texto: 'Documentación', tipo: 'info' }
    ]
  }
])

// Funciones
function toggleDetalle(id) {
  const informe = informes.value.find(i => i.id === id)
  if (informe) {
    informe.mostrarDetalle = !informe.mostrarDetalle
  }
}

function formatoFecha(fecha) {
  return new Date(fecha).toLocaleDateString('es-AR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

// Columnas para exportación
const columnasExportacion = [
  { id: 'titulo', nombre: 'Título' },
  { id: 'tipo', nombre: 'Tipo' },
  { id: 'fechaGeneracion', nombre: 'Fecha de Generación' },
  { id: 'estado', nombre: 'Estado' },
  { id: 'usuario', nombre: 'Usuario' },
  { id: 'descripcion', nombre: 'Descripción' }
]
</script>

<style>
/* Estilos para impresión */
@media print {
  .no-print {
    display: none !important;
  }

  .prose {
    max-width: none !important;
  }

  .prose h4 {
    margin-top: 1.5em;
    margin-bottom: 0.5em;
  }

  .prose ul {
    margin-top: 0.5em;
    margin-bottom: 1em;
  }

  .prose li {
    margin-top: 0.25em;
  }
}

/* Estilos para mobile */
@media (max-width: 640px) {
  .prose {
    font-size: 0.875rem;
  }

  .prose h4 {
    font-size: 1rem;
  }

  .prose ul {
    padding-left: 1.25rem;
  }
}
</style> 