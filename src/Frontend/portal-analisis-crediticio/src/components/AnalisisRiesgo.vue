<template>
  <div>
    <!-- Botón de análisis -->
    <button
      @click="mostrarConfirmacion = true"
      class="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
      </svg>
      Analizar Riesgo
    </button>

    <!-- Modal de confirmación -->
    <div
      v-if="mostrarConfirmacion"
      class="fixed inset-0 z-50 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div
          class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"
          aria-hidden="true"
          @click="mostrarConfirmacion = false"
        ></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div class="mx-auto flex-shrink-0 flex items-center justify-center h-12 w-12 rounded-full bg-blue-100 sm:mx-0 sm:h-10 sm:w-10">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                </svg>
              </div>
              <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                <h3 class="text-lg leading-6 font-medium text-gray-900" id="modal-title">
                  Confirmar Análisis de Riesgo
                </h3>
                <div class="mt-2">
                  <p class="text-sm text-gray-500">
                    Se realizará un análisis completo del riesgo crediticio utilizando todas las fuentes disponibles.
                    Este proceso puede tomar unos minutos.
                  </p>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button
              type="button"
              class="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-blue-600 text-base font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:ml-3 sm:w-auto sm:text-sm"
              @click="iniciarAnalisis"
            >
              Iniciar Análisis
            </button>
            <button
              type="button"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
              @click="mostrarConfirmacion = false"
            >
              Cancelar
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de progreso -->
    <div
      v-if="analizando"
      class="fixed inset-0 z-50 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" aria-hidden="true"></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div class="mt-3 text-center sm:mt-0 sm:text-left w-full">
                <h3 class="text-lg leading-6 font-medium text-gray-900" id="modal-title">
                  Analizando Riesgo
                </h3>
                <div class="mt-4">
                  <div class="relative pt-1">
                    <div class="overflow-hidden h-2 mb-4 text-xs flex rounded bg-blue-200">
                      <div
                        class="shadow-none flex flex-col text-center whitespace-nowrap text-white justify-center bg-blue-500 transition-all duration-500"
                        :style="{ width: `${progreso}%` }"
                      ></div>
                    </div>
                    <div class="flex justify-between text-sm text-gray-600">
                      <span>Progreso</span>
                      <span>{{ progreso }}%</span>
                    </div>
                  </div>
                  <p class="mt-2 text-sm text-gray-500">
                    {{ mensajeProgreso }}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de resultados -->
    <div
      v-if="mostrarResultados"
      class="fixed inset-0 z-50 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true"
    >
      <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
        <div
          class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"
          aria-hidden="true"
          @click="mostrarResultados = false"
        ></div>

        <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>

        <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-2xl sm:w-full">
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="sm:flex sm:items-start">
              <div class="mt-3 text-center sm:mt-0 sm:text-left w-full">
                <!-- Nivel de riesgo -->
                <div class="mb-6">
                  <h3 class="text-lg font-medium text-gray-900 mb-2">Nivel de Riesgo</h3>
                  <div
                    class="inline-flex items-center px-4 py-2 rounded-full text-lg font-medium"
                    :class="{
                      'bg-red-100 text-red-800': resultado.nivel === 'Alto',
                      'bg-yellow-100 text-yellow-800': resultado.nivel === 'Medio',
                      'bg-green-100 text-green-800': resultado.nivel === 'Bajo'
                    }"
                  >
                    {{ resultado.nivel }}
                  </div>
                </div>

                <!-- Factores clave -->
                <div class="mb-6">
                  <h3 class="text-lg font-medium text-gray-900 mb-2">Factores Clave</h3>
                  <ul class="space-y-2">
                    <li
                      v-for="(factor, index) in resultado.factores"
                      :key="index"
                      class="flex items-start"
                    >
                      <span
                        class="flex-shrink-0 h-5 w-5 mr-2"
                        :class="{
                          'text-red-500': factor.impacto === 'negativo',
                          'text-green-500': factor.impacto === 'positivo',
                          'text-yellow-500': factor.impacto === 'neutral'
                        }"
                      >
                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                          <path
                            v-if="factor.impacto === 'negativo'"
                            fill-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z"
                            clip-rule="evenodd"
                          />
                          <path
                            v-if="factor.impacto === 'positivo'"
                            fill-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
                            clip-rule="evenodd"
                          />
                          <path
                            v-if="factor.impacto === 'neutral'"
                            fill-rule="evenodd"
                            d="M10 18a8 8 0 100-16 8 8 0 000 16zm1-11a1 1 0 10-2 0v2H7a1 1 0 100 2h2v2a1 1 0 102 0v-2h2a1 1 0 100-2h-2V7z"
                            clip-rule="evenodd"
                          />
                        </svg>
                      </span>
                      <span class="text-sm text-gray-700">{{ factor.descripcion }}</span>
                    </li>
                  </ul>
                </div>

                <!-- Recomendación -->
                <div class="mb-6">
                  <h3 class="text-lg font-medium text-gray-900 mb-2">Recomendación</h3>
                  <p class="text-sm text-gray-700">{{ resultado.recomendacion }}</p>
                </div>

                <!-- Link al informe -->
                <div class="border-t border-gray-200 pt-4">
                  <a
                    href="#"
                    class="inline-flex items-center text-sm font-medium text-blue-600 hover:text-blue-800"
                  >
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                    </svg>
                    Ver informe completo
                  </a>
                </div>
              </div>
            </div>
          </div>
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <button
              type="button"
              class="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
              @click="mostrarResultados = false"
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
import { ref } from 'vue'

// Estado
const mostrarConfirmacion = ref(false)
const analizando = ref(false)
const mostrarResultados = ref(false)
const progreso = ref(0)
const mensajeProgreso = ref('Iniciando análisis...')

// Resultado simulado
const resultado = ref({
  nivel: 'Medio',
  factores: [
    {
      descripcion: 'Score crediticio por debajo del promedio',
      impacto: 'negativo'
    },
    {
      descripcion: 'Sin deudas vencidas en los últimos 12 meses',
      impacto: 'positivo'
    },
    {
      descripcion: 'Antigüedad laboral de 3 años',
      impacto: 'neutral'
    },
    {
      descripcion: 'Ingresos estables y verificables',
      impacto: 'positivo'
    }
  ],
  recomendacion: 'Se recomienda aprobar el crédito con un límite moderado y seguimiento trimestral de la situación crediticia.'
})

// Funciones
function iniciarAnalisis() {
  mostrarConfirmacion.value = false
  analizando.value = true
  progreso.value = 0
  mensajeProgreso.value = 'Iniciando análisis...'

  // Simular progreso
  const interval = setInterval(() => {
    progreso.value += 10
    if (progreso.value <= 30) {
      mensajeProgreso.value = 'Consultando Veraz...'
    } else if (progreso.value <= 60) {
      mensajeProgreso.value = 'Analizando BCRA...'
    } else if (progreso.value <= 90) {
      mensajeProgreso.value = 'Procesando AFIP...'
    } else {
      mensajeProgreso.value = 'Finalizando análisis...'
    }

    if (progreso.value >= 100) {
      clearInterval(interval)
      setTimeout(() => {
        analizando.value = false
        mostrarResultados.value = true
      }, 500)
    }
  }, 500)
}
</script> 