<template>
  <div>
    <h2 class="text-xl font-semibold text-gray-900 mb-6">Selección de Cliente</h2>
    
    <!-- Buscador de clientes -->
    <div class="relative">
      <input
        type="text"
        v-model="busqueda"
        placeholder="Buscar cliente por nombre o CUIT..."
        class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        @input="buscarClientes"
        @focus="mostrarSugerencias = true"
      />
      
      <!-- Icono de búsqueda -->
      <div class="absolute inset-y-0 right-0 flex items-center pr-3">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
        </svg>
      </div>
      
      <!-- Lista de sugerencias -->
      <div
        v-if="mostrarSugerencias && sugerencias.length > 0"
        class="absolute z-10 w-full mt-1 bg-white rounded-lg shadow-lg border border-gray-200 max-h-60 overflow-y-auto"
      >
        <div
          v-for="cliente in sugerencias"
          :key="cliente.id"
          class="px-4 py-2 hover:bg-gray-50 cursor-pointer"
          @click="seleccionarCliente(cliente)"
        >
          <div class="font-medium text-gray-900">{{ cliente.nombre }}</div>
          <div class="text-sm text-gray-500">CUIT: {{ cliente.cuit }}</div>
        </div>
      </div>
    </div>
    
    <!-- Cliente seleccionado -->
    <div
      v-if="datos.cliente"
      class="mt-6 p-4 bg-gray-50 rounded-lg"
    >
      <div class="flex justify-between items-start">
        <div>
          <h3 class="text-lg font-medium text-gray-900">{{ datos.cliente.nombre }}</h3>
          <p class="text-sm text-gray-500">CUIT: {{ datos.cliente.cuit }}</p>
          <p class="text-sm text-gray-500 mt-1">
            {{ datos.cliente.direccion }} - {{ datos.cliente.localidad }}
          </p>
        </div>
        <button
          type="button"
          class="text-gray-400 hover:text-gray-500"
          @click="eliminarCliente"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </div>
    
    <!-- Mensaje de error -->
    <p
      v-if="error"
      class="mt-2 text-sm text-red-600"
    >
      {{ error }}
    </p>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { clienteService } from '../../services/clienteService'
import debounce from 'lodash/debounce'

const props = defineProps({
  datos: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['update:datos', 'validar'])

const busqueda = ref('')
const sugerencias = ref([])
const mostrarSugerencias = ref(false)
const error = ref('')

// Buscar clientes con debounce
const buscarClientes = debounce(async () => {
  if (busqueda.value.length < 3) {
    sugerencias.value = []
    return
  }
  
  try {
    const resultado = await clienteService.getClientes({
      busqueda: busqueda.value
    })
    sugerencias.value = resultado.data
  } catch (err) {
    console.error('Error al buscar clientes:', err)
    error.value = 'Error al buscar clientes'
  }
}, 300)

// Seleccionar cliente
function seleccionarCliente(cliente) {
  props.datos.cliente = cliente
  busqueda.value = ''
  mostrarSugerencias.value = false
  error.value = ''
  validarFormulario()
}

// Eliminar cliente seleccionado
function eliminarCliente() {
  props.datos.cliente = null
  validarFormulario()
}

// Validar formulario
function validarFormulario() {
  const esValido = !!props.datos.cliente
  emit('validar', esValido)
}

// Cerrar sugerencias al hacer clic fuera
function cerrarSugerencias(e) {
  if (!e.target.closest('.relative')) {
    mostrarSugerencias.value = false
  }
}

// Agregar y remover event listener
onMounted(() => {
  document.addEventListener('click', cerrarSugerencias)
})

onUnmounted(() => {
  document.removeEventListener('click', cerrarSugerencias)
})

// Validar al montar el componente
onMounted(validarFormulario)
</script> 