<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40">
    <div class="bg-white rounded-xl shadow-lg w-full max-w-2xl p-6 relative animate-fadeIn">
      <button class="absolute top-3 right-3 text-gray-400 hover:text-gray-600" @click="$emit('close')">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>

      <h2 class="text-xl font-bold mb-4 text-gray-800">Importar Excel</h2>

      <!-- Área de drag & drop -->
      <div
        class="border-2 border-dashed border-gray-300 rounded-lg p-8 text-center"
        :class="{ 'border-blue-500 bg-blue-50': isDragging }"
        @dragenter.prevent="isDragging = true"
        @dragleave.prevent="isDragging = false"
        @dragover.prevent
        @drop.prevent="onFileDrop"
      >
        <div class="space-y-4">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12 mx-auto text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" />
          </svg>
          <div class="text-gray-600">
            <p class="font-medium">Arrastra y suelta tu archivo Excel aquí</p>
            <p class="text-sm">o</p>
            <label class="mt-2 inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 cursor-pointer">
              <span>Seleccionar archivo</span>
              <input type="file" class="hidden" accept=".xlsx,.xls" @change="onFileSelect" />
            </label>
          </div>
        </div>
      </div>

      <!-- Preview del archivo -->
      <div v-if="previewData" class="mt-6">
        <h3 class="text-lg font-semibold mb-4">Vista previa</h3>
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th v-for="header in previewData.headers" :key="header" class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                  {{ header }}
                </th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="(row, index) in previewData.rows" :key="index">
                <td v-for="header in previewData.headers" :key="header" class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {{ row[header] }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Botones de acción -->
      <div class="mt-6 flex justify-end gap-4">
        <button
          class="px-4 py-2 text-gray-600 hover:text-gray-800"
          @click="$emit('close')"
        >
          Cancelar
        </button>
        <button
          class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
          :disabled="!previewData"
          @click="importData"
        >
          Importar
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useToast } from 'vue-toastification'
import * as XLSX from 'xlsx'

const emit = defineEmits(['close', 'imported'])
const toast = useToast()

const isDragging = ref(false)
const previewData = ref(null)
const file = ref(null)

function onFileDrop(e) {
  isDragging.value = false
  const droppedFile = e.dataTransfer.files[0]
  handleFile(droppedFile)
}

function onFileSelect(e) {
  const selectedFile = e.target.files[0]
  handleFile(selectedFile)
}

function handleFile(file) {
  if (!file) return

  // Validar tipo de archivo
  if (!file.name.match(/\.(xlsx|xls)$/)) {
    toast.error('Por favor, selecciona un archivo Excel válido')
    return
  }

  file.value = file
  readExcelFile(file)
}

function readExcelFile(file) {
  const reader = new FileReader()
  
  reader.onload = (e) => {
    try {
      const data = new Uint8Array(e.target.result)
      const workbook = XLSX.read(data, { type: 'array' })
      const firstSheet = workbook.Sheets[workbook.SheetNames[0]]
      const jsonData = XLSX.utils.sheet_to_json(firstSheet)

      if (jsonData.length === 0) {
        toast.error('El archivo está vacío')
        return
      }

      // Preparar datos para preview
      const headers = Object.keys(jsonData[0])
      previewData.value = {
        headers,
        rows: jsonData.slice(0, 5) // Mostrar solo las primeras 5 filas
      }
    } catch (error) {
      console.error('Error al leer el archivo:', error)
      toast.error('Error al leer el archivo Excel')
    }
  }

  reader.onerror = () => {
    toast.error('Error al leer el archivo')
  }

  reader.readAsArrayBuffer(file)
}

function importData() {
  if (!previewData.value) return

  try {
    // Procesar los datos según la estructura esperada
    const processedData = {
      balance: {},
      resultados: {},
      cashflow: {},
      deudas: {}
    }

    // Mapear los datos del Excel a la estructura del formulario
    previewData.value.rows.forEach(row => {
      // Aquí deberías mapear los campos según la estructura de tu Excel
      // Este es un ejemplo, ajusta según tu estructura
      if (row.tipo === 'balance') {
        processedData.balance[row.campo] = row.valor
      } else if (row.tipo === 'resultados') {
        processedData.resultados[row.campo] = row.valor
      } else if (row.tipo === 'cashflow') {
        processedData.cashflow[row.campo] = row.valor
      } else if (row.tipo === 'deudas') {
        processedData.deudas[row.campo] = row.valor
      }
    })

    emit('imported', processedData)
    emit('close')
  } catch (error) {
    console.error('Error al procesar los datos:', error)
    toast.error('Error al procesar los datos del Excel')
  }
}
</script>

<style scoped>
.animate-fadeIn {
  animation: fadeIn 0.2s ease;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style> 