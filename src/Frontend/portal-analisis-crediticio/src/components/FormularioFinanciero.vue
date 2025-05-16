<template>
  <div class="space-y-6">
    <!-- Encabezado con botón de importación -->
    <div class="flex justify-between items-center">
      <h3 class="text-lg font-semibold text-gray-800">{{ titulo }}</h3>
      <button
        class="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
        @click="mostrarImportModal = true"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12" />
        </svg>
        Importar Excel
      </button>
    </div>

    <!-- Formulario -->
    <form @submit.prevent="onSubmit" class="space-y-6">
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div v-for="(campo, key) in campos" :key="key" class="space-y-2">
          <label :for="key" class="block text-sm font-medium text-gray-700">
            {{ campo.label }}
            <span v-if="campo.required" class="text-red-500">*</span>
          </label>
          
          <!-- Input numérico con formato de moneda -->
          <div class="relative">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <span class="text-gray-500 sm:text-sm">$</span>
            </div>
            <input
              :id="key"
              v-model="formData[key]"
              type="text"
              :class="[
                'pl-7 pr-12 block w-full rounded-lg border-gray-300 shadow-sm focus:ring-blue-500 focus:border-blue-500 sm:text-sm',
                { 'border-red-300': errores[key] }
              ]"
              :placeholder="campo.placeholder"
              @input="validarCampo(key)"
              @blur="validarCampo(key)"
            />
            <div class="absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none">
              <span class="text-gray-500 sm:text-sm">ARS</span>
            </div>
          </div>

          <!-- Mensaje de error -->
          <p v-if="errores[key]" class="mt-1 text-sm text-red-600">
            {{ errores[key] }}
          </p>

          <!-- Tooltip con información adicional -->
          <div v-if="campo.tooltip" class="group relative">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 text-gray-400 cursor-help" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <div class="hidden group-hover:block absolute z-10 w-64 p-2 mt-1 text-sm text-white bg-gray-900 rounded-lg shadow-lg">
              {{ campo.tooltip }}
            </div>
          </div>
        </div>
      </div>

      <!-- Botones de acción -->
      <div class="flex justify-end space-x-4">
        <button
          type="button"
          class="px-4 py-2 text-gray-700 bg-gray-100 rounded-lg hover:bg-gray-200"
          @click="$emit('cancel')"
        >
          Cancelar
        </button>
        <button
          type="submit"
          class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
          :disabled="!esValido"
        >
          Guardar
        </button>
      </div>
    </form>

    <!-- Modal de importación -->
    <ImportExcelModal
      v-if="mostrarImportModal"
      @close="mostrarImportModal = false"
      @imported="onImportData"
    />
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useToast } from 'vue-toastification'
import ImportExcelModal from './ImportExcelModal.vue'

const props = defineProps({
  tipo: {
    type: String,
    required: true,
    validator: (value) => ['balance', 'resultados', 'cashflow', 'deudas'].includes(value)
  },
  datosIniciales: {
    type: Object,
    default: () => ({})
  }
})

const emit = defineEmits(['submit', 'cancel'])
const toast = useToast()

// Estado del formulario
const formData = ref({ ...props.datosIniciales })
const errores = ref({})
const mostrarImportModal = ref(false)

// Configuración de campos según el tipo
const campos = computed(() => {
  const configuraciones = {
    balance: {
      activoCorriente: {
        label: 'Activo Corriente',
        required: true,
        placeholder: '0.00',
        tooltip: 'Suma de todos los activos que se pueden convertir en efectivo en un año'
      },
      activoNoCorriente: {
        label: 'Activo No Corriente',
        required: true,
        placeholder: '0.00',
        tooltip: 'Activos que no se pueden convertir en efectivo en un año'
      },
      pasivoCorriente: {
        label: 'Pasivo Corriente',
        required: true,
        placeholder: '0.00',
        tooltip: 'Deudas y obligaciones que vencen en un año'
      },
      pasivoNoCorriente: {
        label: 'Pasivo No Corriente',
        required: true,
        placeholder: '0.00',
        tooltip: 'Deudas y obligaciones que vencen en más de un año'
      },
      patrimonioNeto: {
        label: 'Patrimonio Neto',
        required: true,
        placeholder: '0.00',
        tooltip: 'Diferencia entre activos y pasivos'
      }
    },
    resultados: {
      ventas: {
        label: 'Ventas',
        required: true,
        placeholder: '0.00',
        tooltip: 'Ingresos totales por ventas'
      },
      costoVentas: {
        label: 'Costo de Ventas',
        required: true,
        placeholder: '0.00',
        tooltip: 'Costo directo de los productos vendidos'
      },
      gastosOperativos: {
        label: 'Gastos Operativos',
        required: true,
        placeholder: '0.00',
        tooltip: 'Gastos necesarios para la operación del negocio'
      },
      resultadoOperativo: {
        label: 'Resultado Operativo',
        required: true,
        placeholder: '0.00',
        tooltip: 'Beneficio antes de intereses e impuestos'
      },
      resultadoNeto: {
        label: 'Resultado Neto',
        required: true,
        placeholder: '0.00',
        tooltip: 'Beneficio final después de todos los gastos'
      }
    },
    cashflow: {
      flujoOperativo: {
        label: 'Flujo Operativo',
        required: true,
        placeholder: '0.00',
        tooltip: 'Efectivo generado por las operaciones'
      },
      flujoInversiones: {
        label: 'Flujo de Inversiones',
        required: true,
        placeholder: '0.00',
        tooltip: 'Efectivo usado en inversiones'
      },
      flujoFinanciamiento: {
        label: 'Flujo de Financiamiento',
        required: true,
        placeholder: '0.00',
        tooltip: 'Efectivo de préstamos y pagos de deuda'
      },
      efectivoNeto: {
        label: 'Efectivo Neto',
        required: true,
        placeholder: '0.00',
        tooltip: 'Cambio neto en efectivo'
      }
    },
    deudas: {
      deudaCortoPlazo: {
        label: 'Deuda Corto Plazo',
        required: true,
        placeholder: '0.00',
        tooltip: 'Deudas que vencen en menos de un año'
      },
      deudaLargoPlazo: {
        label: 'Deuda Largo Plazo',
        required: true,
        placeholder: '0.00',
        tooltip: 'Deudas que vencen en más de un año'
      },
      intereses: {
        label: 'Intereses',
        required: true,
        placeholder: '0.00',
        tooltip: 'Gastos por intereses'
      },
      ratioEndeudamiento: {
        label: 'Ratio de Endeudamiento',
        required: true,
        placeholder: '0.00',
        tooltip: 'Deuda total / Activos totales'
      }
    }
  }

  return configuraciones[props.tipo]
})

const titulo = computed(() => {
  const titulos = {
    balance: 'Balance General',
    resultados: 'Estado de Resultados',
    cashflow: 'Flujo de Efectivo',
    deudas: 'Análisis de Deudas'
  }
  return titulos[props.tipo]
})

// Validación
function validarCampo(campo) {
  const valor = formData.value[campo]
  const config = campos.value[campo]

  if (config.required && !valor) {
    errores.value[campo] = 'Este campo es requerido'
    return false
  }

  if (valor) {
    // Formatear el valor como número
    const numero = parseFloat(valor.replace(/[^0-9.-]+/g, ''))
    if (isNaN(numero)) {
      errores.value[campo] = 'Debe ser un número válido'
      return false
    }
    formData.value[campo] = numero.toLocaleString('es-AR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    })
  }

  delete errores.value[campo]
  return true
}

const esValido = computed(() => {
  return Object.keys(campos.value).every(campo => !errores.value[campo])
})

// Manejo de importación
function onImportData(data) {
  if (data[props.tipo]) {
    formData.value = { ...formData.value, ...data[props.tipo] }
    toast.success('Datos importados correctamente')
  }
}

// Envío del formulario
function onSubmit() {
  if (!esValido.value) {
    toast.error('Por favor, corrige los errores antes de continuar')
    return
  }

  // Convertir los valores formateados a números
  const datosEnviar = {}
  Object.keys(formData.value).forEach(key => {
    const valor = formData.value[key]
    if (typeof valor === 'string') {
      datosEnviar[key] = parseFloat(valor.replace(/[^0-9.-]+/g, ''))
    } else {
      datosEnviar[key] = valor
    }
  })

  emit('submit', datosEnviar)
}
</script> 