<template>
  <div>
    <h2 class="text-xl font-semibold text-gray-900 mb-6">Condiciones de Pago</h2>
    
    <div class="space-y-6">
      <!-- Plazo -->
      <div>
        <div class="flex justify-between items-center mb-2">
          <label class="block text-sm font-medium text-gray-700">
            Plazo (meses)
          </label>
          <span class="text-sm text-gray-500">{{ datos.condiciones.plazo }} meses</span>
        </div>
        <input
          type="range"
          v-model.number="datos.condiciones.plazo"
          min="1"
          max="60"
          step="1"
          class="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer"
          @input="calcularCuota"
        />
        <div class="flex justify-between text-xs text-gray-500 mt-1">
          <span>1 mes</span>
          <span>60 meses</span>
        </div>
      </div>
      
      <!-- Frecuencia de pago -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Frecuencia de pago
        </label>
        <div class="grid grid-cols-3 gap-4">
          <button
            v-for="frecuencia in frecuencias"
            :key="frecuencia.valor"
            type="button"
            class="px-4 py-2 text-sm font-medium rounded-lg border"
            :class="[
              datos.condiciones.frecuencia === frecuencia.valor
                ? 'bg-blue-50 border-blue-500 text-blue-700'
                : 'border-gray-300 text-gray-700 hover:bg-gray-50'
            ]"
            @click="seleccionarFrecuencia(frecuencia.valor)"
          >
            {{ frecuencia.nombre }}
          </button>
        </div>
      </div>
      
      <!-- Tasa de interés -->
      <div>
        <div class="flex justify-between items-center mb-2">
          <label class="block text-sm font-medium text-gray-700">
            Tasa de interés anual
          </label>
          <span class="text-sm text-gray-500">{{ datos.condiciones.tasa }}%</span>
        </div>
        <input
          type="range"
          v-model.number="datos.condiciones.tasa"
          min="0"
          max="100"
          step="0.1"
          class="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer"
          @input="calcularCuota"
        />
        <div class="flex justify-between text-xs text-gray-500 mt-1">
          <span>0%</span>
          <span>100%</span>
        </div>
      </div>
      
      <!-- Resumen de cuotas -->
      <div class="bg-blue-50 rounded-lg p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Resumen de cuotas</h3>
        
        <div class="grid grid-cols-2 gap-4">
          <div>
            <p class="text-sm text-gray-500">Monto total</p>
            <p class="text-lg font-medium text-gray-900">
              ${{ datos.condiciones.montoTotal.toLocaleString('es-AR') }}
            </p>
          </div>
          
          <div>
            <p class="text-sm text-gray-500">Valor de la cuota</p>
            <p class="text-lg font-medium text-blue-600">
              ${{ valorCuota.toLocaleString('es-AR') }}
            </p>
          </div>
          
          <div>
            <p class="text-sm text-gray-500">Interés total</p>
            <p class="text-lg font-medium text-gray-900">
              ${{ interesTotal.toLocaleString('es-AR') }}
            </p>
          </div>
          
          <div>
            <p class="text-sm text-gray-500">Monto total a pagar</p>
            <p class="text-lg font-medium text-gray-900">
              ${{ montoTotalPagar.toLocaleString('es-AR') }}
            </p>
          </div>
        </div>
        
        <!-- Gráfico de amortización -->
        <div class="mt-6">
          <h4 class="text-sm font-medium text-gray-700 mb-2">Gráfico de amortización</h4>
          <div class="h-4 bg-gray-200 rounded-full overflow-hidden">
            <div
              class="h-full bg-blue-600"
              :style="{ width: `${porcentajeAmortizado}%` }"
            ></div>
          </div>
          <div class="flex justify-between text-xs text-gray-500 mt-1">
            <span>Capital</span>
            <span>Interés</span>
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

// Frecuencias de pago disponibles
const frecuencias = [
  { valor: 'mensual', nombre: 'Mensual' },
  { valor: 'bimestral', nombre: 'Bimestral' },
  { valor: 'trimestral', nombre: 'Trimestral' }
]

// Valores calculados
const valorCuota = ref(0)
const interesTotal = ref(0)
const montoTotalPagar = ref(0)
const porcentajeAmortizado = ref(0)

// Seleccionar frecuencia
function seleccionarFrecuencia(frecuencia) {
  props.datos.condiciones.frecuencia = frecuencia
  calcularCuota()
}

// Calcular cuota y valores relacionados
function calcularCuota() {
  const { montoTotal, plazo, tasa } = props.datos.condiciones
  
  // Convertir tasa anual a mensual
  const tasaMensual = tasa / 12 / 100
  
  // Calcular cuota mensual
  const cuota = (montoTotal * tasaMensual * Math.pow(1 + tasaMensual, plazo)) /
                (Math.pow(1 + tasaMensual, plazo) - 1)
  
  valorCuota.value = cuota
  interesTotal.value = (cuota * plazo) - montoTotal
  montoTotalPagar.value = cuota * plazo
  
  // Calcular porcentaje amortizado
  porcentajeAmortizado.value = (montoTotal / montoTotalPagar.value) * 100
  
  validarFormulario()
}

// Validar formulario
function validarFormulario() {
  const { plazo, frecuencia, tasa, montoTotal } = props.datos.condiciones
  const esValido = plazo > 0 && frecuencia && tasa >= 0 && montoTotal > 0
  emit('validar', esValido)
}

// Calcular cuota al montar el componente
onMounted(calcularCuota)
</script>

<style scoped>
input[type="range"] {
  -webkit-appearance: none;
  appearance: none;
  height: 8px;
  background: #e5e7eb;
  border-radius: 0.5rem;
  outline: none;
}

input[type="range"]::-webkit-slider-thumb {
  -webkit-appearance: none;
  appearance: none;
  width: 20px;
  height: 20px;
  background: #2563eb;
  border-radius: 50%;
  cursor: pointer;
  transition: background 0.15s ease-in-out;
}

input[type="range"]::-webkit-slider-thumb:hover {
  background: #1d4ed8;
}

input[type="range"]::-moz-range-thumb {
  width: 20px;
  height: 20px;
  background: #2563eb;
  border-radius: 50%;
  cursor: pointer;
  transition: background 0.15s ease-in-out;
  border: none;
}

input[type="range"]::-moz-range-thumb:hover {
  background: #1d4ed8;
}
</style> 