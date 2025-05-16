<template>
  <div class="max-w-4xl mx-auto">
    <!-- Barra de progreso -->
    <div class="mb-8">
      <div class="flex justify-between items-center">
        <div
          v-for="(paso, index) in pasos"
          :key="paso.id"
          class="flex items-center"
        >
          <!-- Círculo del paso -->
          <div
            class="w-10 h-10 rounded-full flex items-center justify-center"
            :class="[
              pasoActual >= index
                ? 'bg-blue-600 text-white'
                : 'bg-gray-200 text-gray-600'
            ]"
          >
            <span v-if="pasoActual > index">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
              </svg>
            </span>
            <span v-else>{{ index + 1 }}</span>
          </div>
          
          <!-- Línea conectora -->
          <div
            v-if="index < pasos.length - 1"
            class="w-24 h-1"
            :class="pasoActual > index ? 'bg-blue-600' : 'bg-gray-200'"
          ></div>
        </div>
      </div>
      
      <!-- Nombres de los pasos -->
      <div class="flex justify-between mt-2">
        <div
          v-for="paso in pasos"
          :key="paso.id"
          class="text-sm font-medium"
          :class="pasoActual >= pasos.indexOf(paso) ? 'text-blue-600' : 'text-gray-500'"
        >
          {{ paso.nombre }}
        </div>
      </div>
    </div>

    <!-- Contenido del paso actual -->
    <div class="bg-white rounded-xl shadow-sm p-6">
      <component
        :is="pasoActualComponent"
        v-model:datos="datosFormulario"
        @validar="validarPasoActual"
      />
    </div>

    <!-- Botones de navegación -->
    <div class="flex justify-between mt-6">
      <button
        v-if="pasoActual > 0"
        type="button"
        class="px-6 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50"
        @click="pasoAnterior"
      >
        Anterior
      </button>
      <div v-else></div>
      
      <button
        type="button"
        class="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        :disabled="!pasoValido"
        @click="siguientePaso"
      >
        {{ esUltimoPaso ? 'Enviar Solicitud' : 'Siguiente' }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useToast } from 'vue-toastification'
import SeleccionCliente from './solicitud/SeleccionCliente.vue'
import ProductosFinanciar from './solicitud/ProductosFinanciar.vue'
import CondicionesPago from './solicitud/CondicionesPago.vue'
import ConfirmacionSolicitud from './solicitud/ConfirmacionSolicitud.vue'

const toast = useToast()
const pasoActual = ref(0)
const pasoValido = ref(false)
const datosFormulario = ref({
  cliente: null,
  productos: [],
  condiciones: {
    plazo: 12,
    frecuencia: 'mensual',
    tasa: 0,
    montoTotal: 0
  }
})

// Definición de pasos
const pasos = [
  { id: 'cliente', nombre: 'Cliente' },
  { id: 'productos', nombre: 'Productos' },
  { id: 'condiciones', nombre: 'Condiciones' },
  { id: 'confirmacion', nombre: 'Confirmación' }
]

// Componente del paso actual
const pasoActualComponent = computed(() => {
  switch (pasoActual.value) {
    case 0:
      return SeleccionCliente
    case 1:
      return ProductosFinanciar
    case 2:
      return CondicionesPago
    case 3:
      return ConfirmacionSolicitud
    default:
      return null
  }
})

// Verificar si es el último paso
const esUltimoPaso = computed(() => pasoActual.value === pasos.length - 1)

// Funciones de navegación
function pasoAnterior() {
  if (pasoActual.value > 0) {
    pasoActual.value--
    pasoValido.value = true
  }
}

function siguientePaso() {
  if (esUltimoPaso.value) {
    enviarSolicitud()
  } else {
    pasoActual.value++
    pasoValido.value = false
  }
}

function validarPasoActual(esValido) {
  pasoValido.value = esValido
}

async function enviarSolicitud() {
  try {
    // Aquí iría la lógica para enviar la solicitud al backend
    toast.success('Solicitud enviada correctamente')
    // Redirigir a la página de confirmación o dashboard
  } catch (error) {
    console.error('Error al enviar solicitud:', error)
    toast.error('Error al enviar la solicitud')
  }
}
</script> 