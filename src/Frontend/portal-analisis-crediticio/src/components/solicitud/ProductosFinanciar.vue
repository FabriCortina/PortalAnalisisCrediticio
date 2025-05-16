<template>
  <div>
    <h2 class="text-xl font-semibold text-gray-900 mb-6">Productos a Financiar</h2>
    
    <!-- Buscador de productos -->
    <div class="relative mb-6">
      <input
        type="text"
        v-model="busqueda"
        placeholder="Buscar producto..."
        class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
        @input="buscarProductos"
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
          v-for="producto in sugerencias"
          :key="producto.id"
          class="px-4 py-2 hover:bg-gray-50 cursor-pointer"
          @click="agregarProducto(producto)"
        >
          <div class="font-medium text-gray-900">{{ producto.nombre }}</div>
          <div class="text-sm text-gray-500">
            {{ producto.categoria }} - ${{ producto.precio.toLocaleString('es-AR') }}
          </div>
        </div>
      </div>
    </div>
    
    <!-- Lista de productos seleccionados -->
    <div class="space-y-4">
      <div
        v-for="(producto, index) in datos.productos"
        :key="producto.id"
        class="flex items-center justify-between p-4 bg-gray-50 rounded-lg"
      >
        <div class="flex items-center space-x-4">
          <div class="flex-shrink-0">
            <LazyImage
              :src="producto.imagen"
              :alt="producto.nombre"
              :aspect-ratio="100"
              rounded
            />
          </div>
          <div>
            <h3 class="font-medium text-gray-900">{{ producto.nombre }}</h3>
            <p class="text-sm text-gray-500">{{ producto.categoria }}</p>
            <p class="text-sm font-medium text-gray-900 mt-1">
              ${{ producto.precio.toLocaleString('es-AR') }}
            </p>
          </div>
        </div>
        
        <div class="flex items-center space-x-4">
          <!-- Cantidad -->
          <div class="flex items-center space-x-2">
            <button
              type="button"
              class="p-1 text-gray-400 hover:text-gray-500"
              @click="actualizarCantidad(index, producto.cantidad - 1)"
              :disabled="producto.cantidad <= 1"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 12H4" />
              </svg>
            </button>
            <span class="text-gray-900">{{ producto.cantidad }}</span>
            <button
              type="button"
              class="p-1 text-gray-400 hover:text-gray-500"
              @click="actualizarCantidad(index, producto.cantidad + 1)"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
              </svg>
            </button>
          </div>
          
          <!-- Eliminar -->
          <button
            type="button"
            class="text-gray-400 hover:text-gray-500"
            @click="eliminarProducto(index)"
          >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </div>
      </div>
    </div>
    
    <!-- Resumen -->
    <div class="mt-6 p-4 bg-blue-50 rounded-lg">
      <div class="flex justify-between items-center">
        <div>
          <h3 class="text-lg font-medium text-gray-900">Total</h3>
          <p class="text-sm text-gray-500">{{ datos.productos.length }} productos</p>
        </div>
        <div class="text-right">
          <p class="text-2xl font-bold text-blue-600">
            ${{ calcularTotal().toLocaleString('es-AR') }}
          </p>
        </div>
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
import { ref, watch, onMounted, onUnmounted, computed } from 'vue'
import debounce from 'lodash/debounce'
import LazyImage from '@/components/LazyImage.vue'

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

// Buscar productos con debounce
const buscarProductos = debounce(async () => {
  if (busqueda.value.length < 3) {
    sugerencias.value = []
    return
  }
  
  try {
    // Aquí iría la llamada al servicio para buscar productos
    const resultado = await fetch(`/api/productos?busqueda=${busqueda.value}`)
    sugerencias.value = await resultado.json()
  } catch (err) {
    console.error('Error al buscar productos:', err)
    error.value = 'Error al buscar productos'
  }
}, 300)

// Agregar producto
function agregarProducto(producto) {
  // Verificar si el producto ya está en la lista
  const index = props.datos.productos.findIndex(p => p.id === producto.id)
  
  if (index === -1) {
    props.datos.productos.push({
      ...producto,
      cantidad: 1
    })
  } else {
    props.datos.productos[index].cantidad++
  }
  
  busqueda.value = ''
  mostrarSugerencias.value = false
  error.value = ''
  validarFormulario()
}

// Actualizar cantidad
function actualizarCantidad(index, nuevaCantidad) {
  if (nuevaCantidad > 0) {
    props.datos.productos[index].cantidad = nuevaCantidad
    validarFormulario()
  }
}

// Eliminar producto
function eliminarProducto(index) {
  props.datos.productos.splice(index, 1)
  validarFormulario()
}

// Calcular total
function calcularTotal() {
  return props.datos.productos.reduce((total, producto) => {
    return total + (producto.precio * producto.cantidad)
  }, 0)
}

// Validar formulario
function validarFormulario() {
  const esValido = props.datos.productos.length > 0
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

// Actualizar monto total en condiciones
watch(() => calcularTotal(), (nuevoTotal) => {
  props.datos.condiciones.montoTotal = nuevoTotal
})
</script> 