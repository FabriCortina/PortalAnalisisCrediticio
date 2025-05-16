<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-100 to-blue-50 p-4">
    <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between mb-4 gap-2">
      <h1 class="text-2xl font-bold text-gray-800">Clientes</h1>
      <div class="flex-1 flex items-center gap-2">
        <div class="relative w-full max-w-xs">
          <input
            v-model="search"
            @input="onSearchInput"
            type="text"
            placeholder="Buscar por CUIT o nombre..."
            class="w-full rounded-lg border border-gray-300 pl-10 pr-4 py-2 focus:ring-2 focus:ring-blue-400 focus:outline-none shadow-sm"
          />
          <span class="absolute left-3 top-2.5 text-gray-400">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-4.35-4.35m0 0A7.5 7.5 0 104.5 4.5a7.5 7.5 0 0012.15 12.15z" /></svg>
          </span>
        </div>
      </div>
      <button
        class="hidden sm:inline-flex items-center gap-2 bg-blue-600 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-700 transition"
        @click="openModal"
      >
        <span>Nuevo cliente</span>
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" /></svg>
      </button>
    </div>

    <!-- Tabla de clientes -->
    <div class="overflow-x-auto rounded-lg shadow bg-white">
      <table class="min-w-full divide-y divide-gray-200">
        <thead class="bg-gray-50 sticky top-0 z-10">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nombre</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">CUIT</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Teléfono</th>
            <th class="px-6 py-3"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="cliente in clientes" :key="cliente.id" class="hover:bg-blue-50 transition">
            <td class="px-6 py-4 whitespace-nowrap">{{ cliente.nombre }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ cliente.cuit }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ cliente.email }}</td>
            <td class="px-6 py-4 whitespace-nowrap">{{ cliente.telefono }}</td>
            <td class="px-6 py-4 whitespace-nowrap text-right">
              <!-- Acciones (editar/eliminar) -->
              <button class="text-blue-600 hover:underline mr-2" @click="editCliente(cliente)">Editar</button>
            </td>
          </tr>
          <tr v-if="clientes.length === 0">
            <td colspan="5" class="text-center py-8 text-gray-400">No se encontraron clientes</td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Paginación -->
    <div class="flex flex-col sm:flex-row items-center justify-between mt-4 gap-2">
      <div class="text-sm text-gray-600">
        Mostrando {{ startItem }}-{{ endItem }} de {{ totalClientes }} clientes
      </div>
      <div class="flex gap-2">
        <button :disabled="pagina === 1" @click="pagina--" class="px-3 py-1 rounded border border-gray-300 bg-white hover:bg-gray-100 disabled:opacity-50">Anterior</button>
        <span class="px-2">Página {{ pagina }}</span>
        <button :disabled="endItem >= totalClientes" @click="pagina++" class="px-3 py-1 rounded border border-gray-300 bg-white hover:bg-gray-100 disabled:opacity-50">Siguiente</button>
      </div>
    </div>

    <!-- Botón fijo en mobile -->
    <button
      class="sm:hidden fixed bottom-6 right-6 z-50 bg-blue-600 text-white p-4 rounded-full shadow-lg hover:bg-blue-700 transition"
      @click="openModal"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" /></svg>
    </button>

    <!-- Modal multistep -->
    <NuevoClienteModal
      v-if="showModal"
      :clienteEdit="clienteEdit"
      @close="closeModal"
      @saved="onClienteSaved"
    />

    <!-- Encabezado -->
    <div class="mb-8 flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Clientes</h1>
        <p class="mt-1 text-sm text-gray-500">
          Gestión de clientes del sistema
        </p>
      </div>
      <div class="flex space-x-4">
        <ExportButton
          :columnas="columnasExportacion"
          :datos="clientes"
          nombre-archivo="clientes"
        />
        <button
          @click="nuevoCliente"
          class="inline-flex items-center px-4 py-2 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
        >
          <svg
            class="-ml-1 mr-2 h-5 w-5"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 20 20"
            fill="currentColor"
          >
            <path
              fill-rule="evenodd"
              d="M10 3a1 1 0 011 1v5h5a1 1 0 110 2h-5v5a1 1 0 11-2 0v-5H4a1 1 0 110-2h5V4a1 1 0 011-1z"
              clip-rule="evenodd"
            />
          </svg>
          Nuevo Cliente
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { useToast } from 'vue-toastification'
import NuevoClienteModal from '@/components/NuevoClienteModal.vue'
import clienteService from '@/services/clienteService'
import ExportButton from '@/components/ExportButton.vue'
import { useRouter } from 'vue-router'

const toast = useToast()
const search = ref('')
const clientes = ref([])
const totalClientes = ref(0)
const pagina = ref(1)
const itemsPorPagina = 10
const showModal = ref(false)
const clienteEdit = ref(null)
const loading = ref(false)
let debounceTimeout = null

const startItem = computed(() => (pagina.value - 1) * itemsPorPagina + 1)
const endItem = computed(() => Math.min(pagina.value * itemsPorPagina, totalClientes.value))

function onSearchInput() {
  clearTimeout(debounceTimeout)
  debounceTimeout = setTimeout(() => {
    pagina.value = 1
    loadClientes()
  }, 400)
}

function openModal() {
  clienteEdit.value = null
  showModal.value = true
}

function editCliente(cliente) {
  clienteEdit.value = { ...cliente }
  showModal.value = true
}

function closeModal() {
  showModal.value = false
}

async function onClienteSaved() {
  toast.success('Cliente guardado correctamente')
  showModal.value = false
  loadClientes()
}

async function loadClientes() {
  loading.value = true
  try {
    const filtros = { search: search.value }
    const { data, total } = await clienteService.getClientes(filtros, pagina.value, itemsPorPagina)
    clientes.value = data
    totalClientes.value = total
  } catch (e) {
    clientes.value = []
    totalClientes.value = 0
    toast.error('Error al cargar clientes')
  } finally {
    loading.value = false
  }
}

// Cargar clientes al montar y cuando cambia la página
watch([pagina], loadClientes, { immediate: true })

// Columnas para exportación
const columnasExportacion = [
  { id: 'nombre', nombre: 'Nombre' },
  { id: 'cuit', nombre: 'CUIT' },
  { id: 'email', nombre: 'Email' },
  { id: 'telefono', nombre: 'Teléfono' },
  { id: 'direccion', nombre: 'Dirección' },
  { id: 'estado', nombre: 'Estado' },
  { id: 'fechaAlta', nombre: 'Fecha de Alta' }
]
</script>

<style scoped>
</style> 