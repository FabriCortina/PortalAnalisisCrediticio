<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40">
    <div class="bg-white rounded-xl shadow-lg w-full max-w-lg p-6 relative animate-fadeIn">
      <button class="absolute top-3 right-3 text-gray-400 hover:text-gray-600" @click="$emit('close')">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
      </button>
      <h2 class="text-xl font-bold mb-4 text-gray-800">{{ isEdit ? 'Editar Cliente' : 'Nuevo Cliente' }}</h2>

      <!-- Steps -->
      <div class="flex items-center justify-between mb-6">
        <template v-for="(step, idx) in steps" :key="step">
          <div class="flex items-center">
            <div :class="['w-8 h-8 flex items-center justify-center rounded-full border-2',
              idx === currentStep ? 'border-blue-600 bg-blue-50 text-blue-600' : idx < currentStep ? 'border-blue-400 bg-blue-100 text-blue-400' : 'border-gray-300 bg-gray-100 text-gray-400']">
              <span class="font-bold">{{ idx + 1 }}</span>
            </div>
            <div v-if="idx < steps.length - 1" class="w-8 h-1 bg-gray-200 mx-1 rounded"></div>
          </div>
        </template>
      </div>

      <form @submit.prevent="onSubmit">
        <div v-if="currentStep === 0">
          <!-- Datos personales -->
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Nombre
              <span class="text-gray-400 cursor-pointer" title="Nombre completo del cliente.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <div class="relative">
              <input v-model="form.nombre" @input="validateField('nombre')" type="text" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('nombre')" />
              <span class="absolute right-3 top-3">
                <svg v-if="valid.nombre" class="h-5 w-5 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <svg v-else-if="form.nombre" class="h-5 w-5 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </span>
            </div>
            <p v-if="form.nombre && !valid.nombre" class="text-xs text-red-500 mt-1">El nombre es obligatorio.</p>
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Apellido
              <span class="text-gray-400 cursor-pointer" title="Apellido del cliente.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <div class="relative">
              <input v-model="form.apellido" @input="validateField('apellido')" type="text" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('apellido')" />
              <span class="absolute right-3 top-3">
                <svg v-if="valid.apellido" class="h-5 w-5 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <svg v-else-if="form.apellido" class="h-5 w-5 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </span>
            </div>
            <p v-if="form.apellido && !valid.apellido" class="text-xs text-red-500 mt-1">El apellido es obligatorio.</p>
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              CUIT
              <span class="text-gray-400 cursor-pointer" title="Clave Única de Identificación Tributaria (11 dígitos)">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <div class="relative">
              <input v-model="form.cuit" @input="validateField('cuit')" type="text" maxlength="11" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('cuit')" />
              <span class="absolute right-3 top-3">
                <svg v-if="valid.cuit" class="h-5 w-5 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <svg v-else-if="form.cuit" class="h-5 w-5 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </span>
            </div>
            <p v-if="form.cuit && !valid.cuit" class="text-xs text-red-500 mt-1">CUIT inválido (debe tener 11 dígitos).</p>
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Tipo de documento
              <span class="text-gray-400 cursor-pointer" title="Selecciona el tipo de documento.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <select v-model="form.tipoDocumento" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400">
              <option value="DNI">DNI</option>
              <option value="CUIT">CUIT</option>
              <option value="CUIL">CUIL</option>
            </select>
          </div>
        </div>
        <div v-else-if="currentStep === 1">
          <!-- Contacto -->
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Email
              <span class="text-gray-400 cursor-pointer" title="Correo electrónico válido.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <div class="relative">
              <input v-model="form.email" @input="validateField('email')" type="email" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('email')" />
              <span class="absolute right-3 top-3">
                <svg v-if="valid.email" class="h-5 w-5 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <svg v-else-if="form.email" class="h-5 w-5 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </span>
            </div>
            <p v-if="form.email && !valid.email" class="text-xs text-red-500 mt-1">Email inválido.</p>
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Teléfono
              <span class="text-gray-400 cursor-pointer" title="Número de teléfono de contacto.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <div class="relative">
              <input v-model="form.telefono" @input="validateField('telefono')" type="text" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('telefono')" />
              <span class="absolute right-3 top-3">
                <svg v-if="valid.telefono" class="h-5 w-5 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" /></svg>
                <svg v-else-if="form.telefono" class="h-5 w-5 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" /></svg>
              </span>
            </div>
            <p v-if="form.telefono && !valid.telefono" class="text-xs text-red-500 mt-1">El teléfono es obligatorio.</p>
          </div>
        </div>
        <div v-else-if="currentStep === 2">
          <!-- Dirección -->
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Domicilio
              <span class="text-gray-400 cursor-pointer" title="Dirección completa del cliente.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <input v-model="form.direccion" @input="validateField('direccion')" type="text" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('direccion')" />
            <p v-if="form.direccion && !valid.direccion" class="text-xs text-red-500 mt-1">El domicilio es obligatorio.</p>
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Localidad
              <span class="text-gray-400 cursor-pointer" title="Ciudad o localidad.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <input v-model="form.ciudad" @input="validateField('ciudad')" type="text" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('ciudad')" />
            <p v-if="form.ciudad && !valid.ciudad" class="text-xs text-red-500 mt-1">La localidad es obligatoria.</p>
          </div>
          <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 flex items-center gap-1">
              Provincia
              <span class="text-gray-400 cursor-pointer" title="Provincia de residencia.">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M12 20a8 8 0 100-16 8 8 0 000 16z" /></svg>
              </span>
            </label>
            <input v-model="form.provincia" @input="validateField('provincia')" type="text" class="mt-1 block w-full rounded-lg border px-3 py-2 focus:ring-2 focus:ring-blue-400" :class="inputClass('provincia')" />
            <p v-if="form.provincia && !valid.provincia" class="text-xs text-red-500 mt-1">La provincia es obligatoria.</p>
          </div>
        </div>
        <div v-else-if="currentStep === 3">
          <!-- Confirmación -->
          <div class="mb-4">
            <h3 class="text-lg font-semibold mb-2">Revisá los datos antes de guardar</h3>
            <ul class="text-sm text-gray-700 space-y-1">
              <li><strong>Nombre:</strong> {{ form.nombre }}</li>
              <li><strong>Apellido:</strong> {{ form.apellido }}</li>
              <li><strong>CUIT:</strong> {{ form.cuit }}</li>
              <li><strong>Tipo de documento:</strong> {{ form.tipoDocumento }}</li>
              <li><strong>Email:</strong> {{ form.email }}</li>
              <li><strong>Teléfono:</strong> {{ form.telefono }}</li>
              <li><strong>Domicilio:</strong> {{ form.direccion }}</li>
              <li><strong>Localidad:</strong> {{ form.ciudad }}</li>
              <li><strong>Provincia:</strong> {{ form.provincia }}</li>
            </ul>
          </div>
        </div>

        <!-- Navegación de pasos -->
        <div class="flex justify-between mt-6">
          <button type="button" class="px-4 py-2 rounded bg-gray-200 hover:bg-gray-300" @click="prevStep" :disabled="currentStep === 0">Anterior</button>
          <button
            v-if="currentStep < steps.length - 1"
            type="button"
            class="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700"
            @click="nextStep"
            :disabled="!stepValid(currentStep)"
          >Siguiente</button>
          <button
            v-else
            type="submit"
            class="px-4 py-2 rounded bg-green-600 text-white hover:bg-green-700"
            :disabled="saving"
          >{{ saving ? 'Guardando...' : (isEdit ? 'Guardar cambios' : 'Guardar') }}</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, computed, watch, onMounted } from 'vue'
import { useToast } from 'vue-toastification'
import clienteService from '@/services/clienteService'

const props = defineProps({
  clienteEdit: Object
})
const emit = defineEmits(['close', 'saved'])
const toast = useToast()

const steps = ['Datos personales', 'Contacto', 'Dirección', 'Confirmación']
const currentStep = ref(0)
const saving = ref(false)

const form = reactive({
  nombre: '',
  apellido: '',
  cuit: '',
  tipoDocumento: 'DNI',
  email: '',
  telefono: '',
  direccion: '',
  ciudad: '',
  provincia: '',
  pais: 'Argentina',
  codigoPostal: '',
  fechaNacimiento: '',
  estadoCivil: '',
  ocupacion: '',
  nacionalidad: 'Argentina',
  numeroDocumento: ''
})

const valid = reactive({
  nombre: false,
  apellido: false,
  cuit: false,
  email: false,
  telefono: false,
  direccion: false,
  ciudad: false,
  provincia: false,
  codigoPostal: false,
  numeroDocumento: false
})

const isEdit = computed(() => !!props.clienteEdit)

onMounted(() => {
  if (props.clienteEdit) {
    Object.assign(form, props.clienteEdit)
    validateAll()
  }
})

function validateField(field) {
  switch (field) {
    case 'nombre':
      valid.nombre = !!form.nombre.trim()
      break
    case 'apellido':
      valid.apellido = !!form.apellido.trim()
      break
    case 'cuit':
      valid.cuit = /^\d{11}$/.test(form.cuit)
      break
    case 'email':
      valid.email = /^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(form.email)
      break
    case 'telefono':
      valid.telefono = !!form.telefono.trim()
      break
    case 'direccion':
      valid.direccion = !!form.direccion.trim()
      break
    case 'ciudad':
      valid.ciudad = !!form.ciudad.trim()
      break
    case 'provincia':
      valid.provincia = !!form.provincia.trim()
      break
    case 'codigoPostal':
      valid.codigoPostal = !!form.codigoPostal.trim()
      break
    case 'numeroDocumento':
      valid.numeroDocumento = !!form.numeroDocumento.trim()
      break
  }
}

function validateAll() {
  Object.keys(valid).forEach(validateField)
}

function stepValid(step) {
  if (step === 0) return valid.nombre && valid.apellido && valid.cuit && valid.numeroDocumento
  if (step === 1) return valid.email && valid.telefono
  if (step === 2) return valid.direccion && valid.ciudad && valid.provincia && valid.codigoPostal
  return true
}

function inputClass(field) {
  if (!form[field]) return ''
  return valid[field] ? 'border-green-400' : 'border-red-400'
}

function nextStep() {
  if (currentStep.value < steps.length - 1 && stepValid(currentStep.value)) {
    currentStep.value++
  }
}

function prevStep() {
  if (currentStep.value > 0) {
    currentStep.value--
  }
}

async function onSubmit() {
  saving.value = true
  try {
    const clienteData = {
      ...form,
      fechaNacimiento: form.fechaNacimiento ? new Date(form.fechaNacimiento).toISOString() : null,
      fechaCreacion: new Date().toISOString(),
      ultimaActualizacion: new Date().toISOString(),
      activo: true
    }

    if (isEdit.value) {
      await clienteService.updateCliente(props.clienteEdit.id, clienteData)
      toast.success('Cliente actualizado correctamente')
    } else {
      await clienteService.createCliente(clienteData)
      toast.success('Cliente creado correctamente')
    }
    emit('saved')
  } catch (e) {
    toast.error(e.response?.data?.message || 'Error al guardar el cliente')
  } finally {
    saving.value = false
  }
}
</script>

<style scoped>
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
.animate-fadeIn {
  animation: fadeIn 0.2s ease;
}
</style> 