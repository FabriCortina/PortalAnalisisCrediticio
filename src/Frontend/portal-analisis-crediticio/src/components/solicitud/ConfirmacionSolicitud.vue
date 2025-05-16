<template>
  <div>
    <h2 class="text-xl font-semibold text-gray-900 mb-6">Confirmación de Solicitud</h2>
    
    <div class="space-y-6">
      <!-- Información del cliente -->
      <div class="bg-white rounded-lg border border-gray-200 p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Información del Cliente</h3>
        <div class="grid grid-cols-2 gap-4">
          <div>
            <p class="text-sm text-gray-500">Nombre</p>
            <p class="text-base font-medium text-gray-900">{{ datos.cliente.nombre }}</p>
          </div>
          <div>
            <p class="text-sm text-gray-500">CUIT</p>
            <p class="text-base font-medium text-gray-900">{{ datos.cliente.cuit }}</p>
          </div>
          <div>
            <p class="text-sm text-gray-500">Dirección</p>
            <p class="text-base font-medium text-gray-900">{{ datos.cliente.direccion }}</p>
          </div>
          <div>
            <p class="text-sm text-gray-500">Localidad</p>
            <p class="text-base font-medium text-gray-900">{{ datos.cliente.localidad }}</p>
          </div>
        </div>
      </div>
      
      <!-- Productos seleccionados -->
      <div class="bg-white rounded-lg border border-gray-200 p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Productos Seleccionados</h3>
        <div class="space-y-4">
          <div
            v-for="producto in datos.productos"
            :key="producto.id"
            class="flex items-center justify-between p-4 bg-gray-50 rounded-lg"
          >
            <div class="flex items-center space-x-4">
              <div class="flex-shrink-0">
                <img
                  :src="producto.imagen"
                  :alt="producto.nombre"
                  class="h-16 w-16 object-cover rounded-lg"
                />
              </div>
              <div>
                <h4 class="font-medium text-gray-900">{{ producto.nombre }}</h4>
                <p class="text-sm text-gray-500">{{ producto.categoria }}</p>
                <p class="text-sm font-medium text-gray-900 mt-1">
                  Cantidad: {{ producto.cantidad }}
                </p>
              </div>
            </div>
            <p class="text-lg font-medium text-gray-900">
              ${{ (producto.precio * producto.cantidad).toLocaleString('es-AR') }}
            </p>
          </div>
        </div>
      </div>
      
      <!-- Condiciones de pago -->
      <div class="bg-white rounded-lg border border-gray-200 p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Condiciones de Pago</h3>
        <div class="grid grid-cols-2 gap-4">
          <div>
            <p class="text-sm text-gray-500">Plazo</p>
            <p class="text-base font-medium text-gray-900">
              {{ datos.condiciones.plazo }} meses
            </p>
          </div>
          <div>
            <p class="text-sm text-gray-500">Frecuencia</p>
            <p class="text-base font-medium text-gray-900">
              {{ datos.condiciones.frecuencia }}
            </p>
          </div>
          <div>
            <p class="text-sm text-gray-500">Tasa de interés anual</p>
            <p class="text-base font-medium text-gray-900">
              {{ datos.condiciones.tasa }}%
            </p>
          </div>
          <div>
            <p class="text-sm text-gray-500">Valor de la cuota</p>
            <p class="text-base font-medium text-blue-600">
              ${{ valorCuota.toLocaleString('es-AR') }}
            </p>
          </div>
        </div>
      </div>
      
      <!-- Resumen final -->
      <div class="bg-blue-50 rounded-lg p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Resumen Final</h3>
        <div class="space-y-2">
          <div class="flex justify-between">
            <p class="text-gray-600">Subtotal</p>
            <p class="font-medium text-gray-900">
              ${{ datos.condiciones.montoTotal.toLocaleString('es-AR') }}
            </p>
          </div>
          <div class="flex justify-between">
            <p class="text-gray-600">Interés total</p>
            <p class="font-medium text-gray-900">
              ${{ interesTotal.toLocaleString('es-AR') }}
            </p>
          </div>
          <div class="border-t border-blue-200 my-2"></div>
          <div class="flex justify-between">
            <p class="text-lg font-medium text-gray-900">Total a pagar</p>
            <p class="text-lg font-bold text-blue-600">
              ${{ montoTotalPagar.toLocaleString('es-AR') }}
            </p>
          </div>
        </div>
      </div>
      
      <!-- Términos y condiciones -->
      <div class="bg-white rounded-lg border border-gray-200 p-6">
        <div class="flex items-start">
          <div class="flex items-center h-5">
            <input
              type="checkbox"
              v-model="aceptoTerminos"
              class="h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
              @change="validarFormulario"
            />
          </div>
          <div class="ml-3">
            <label class="text-sm text-gray-700">
              Acepto los términos y condiciones del financiamiento
            </label>
            <p class="text-xs text-gray-500 mt-1">
              Al aceptar, confirmo que he leído y comprendido los términos y condiciones
              del financiamiento, incluyendo las tasas de interés y los plazos de pago.
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const props = defineProps({
  datos: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['update:datos', 'validar'])

const aceptoTerminos = ref(false)

// Valores calculados
const valorCuota = computed(() => {
  const { montoTotal, plazo, tasa } = props.datos.condiciones
  const tasaMensual = tasa / 12 / 100
  return (montoTotal * tasaMensual * Math.pow(1 + tasaMensual, plazo)) /
         (Math.pow(1 + tasaMensual, plazo) - 1)
})

const interesTotal = computed(() => {
  return (valorCuota.value * props.datos.condiciones.plazo) - props.datos.condiciones.montoTotal
})

const montoTotalPagar = computed(() => {
  return valorCuota.value * props.datos.condiciones.plazo
})

// Validar formulario
function validarFormulario() {
  const esValido = aceptoTerminos.value
  emit('validar', esValido)
}

// Validar al montar el componente
onMounted(validarFormulario)
</script> 