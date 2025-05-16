<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-blue-50 py-8 px-4">
    <div class="max-w-7xl mx-auto">
      <!-- Encabezado -->
      <div class="mb-6">
        <h1 class="text-2xl font-bold text-gray-800">Configuración</h1>
        <p class="mt-1 text-sm text-gray-600">Gestiona los ajustes y preferencias del sistema</p>
      </div>

      <!-- Contenido -->
      <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <!-- Menú lateral -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-lg shadow overflow-hidden">
            <nav class="divide-y divide-gray-200">
              <button
                v-for="seccion in secciones"
                :key="seccion.id"
                class="w-full px-4 py-3 text-left hover:bg-gray-50 transition-colors"
                :class="{ 'bg-blue-50 text-blue-600': seccionActiva === seccion.id }"
                @click="seccionActiva = seccion.id"
              >
                <div class="flex items-center">
                  <component :is="seccion.icono" class="w-5 h-5 mr-3" />
                  <span class="font-medium">{{ seccion.titulo }}</span>
                </div>
              </button>
            </nav>
          </div>
        </div>

        <!-- Contenido de la sección -->
        <div class="lg:col-span-2">
          <div class="bg-white rounded-lg shadow p-6">
            <!-- Ajustes Generales -->
            <div v-if="seccionActiva === 'general'">
              <h2 class="text-lg font-semibold mb-4">Ajustes Generales</h2>
              <div class="space-y-4">
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Nombre de la Empresa</label>
                  <input
                    type="text"
                    v-model="configuracion.nombreEmpresa"
                    class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  />
                </div>
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Zona Horaria</label>
                  <select
                    v-model="configuracion.zonaHoraria"
                    class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="America/Argentina/Buenos_Aires">Buenos Aires (GMT-3)</option>
                    <option value="America/Argentina/Cordoba">Córdoba (GMT-3)</option>
                    <option value="America/Argentina/Mendoza">Mendoza (GMT-3)</option>
                  </select>
                </div>
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Idioma</label>
                  <select
                    v-model="configuracion.idioma"
                    class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="es">Español</option>
                    <option value="en">English</option>
                  </select>
                </div>
              </div>
            </div>

            <!-- Notificaciones -->
            <div v-if="seccionActiva === 'notificaciones'">
              <h2 class="text-lg font-semibold mb-4">Notificaciones</h2>
              <div class="space-y-4">
                <div class="flex items-center justify-between">
                  <div>
                    <h3 class="font-medium">Notificaciones por Email</h3>
                    <p class="text-sm text-gray-500">Recibe alertas y actualizaciones por correo electrónico</p>
                  </div>
                  <Switch
                    v-model="configuracion.notificacionesEmail"
                    class="ml-4"
                  />
                </div>
                <div class="flex items-center justify-between">
                  <div>
                    <h3 class="font-medium">Notificaciones Push</h3>
                    <p class="text-sm text-gray-500">Recibe notificaciones en tiempo real en el navegador</p>
                  </div>
                  <Switch
                    v-model="configuracion.notificacionesPush"
                    class="ml-4"
                  />
                </div>
                <div class="flex items-center justify-between">
                  <div>
                    <h3 class="font-medium">Recordatorios de Vencimientos</h3>
                    <p class="text-sm text-gray-500">Recibe recordatorios de vencimientos próximos</p>
                  </div>
                  <Switch
                    v-model="configuracion.recordatoriosVencimientos"
                    class="ml-4"
                  />
                </div>
              </div>
            </div>

            <!-- Seguridad -->
            <div v-if="seccionActiva === 'seguridad'">
              <h2 class="text-lg font-semibold mb-4">Seguridad</h2>
              <div class="space-y-4">
                <div class="flex items-center justify-between">
                  <div>
                    <h3 class="font-medium">Autenticación de Dos Factores</h3>
                    <p class="text-sm text-gray-500">Añade una capa extra de seguridad a tu cuenta</p>
                  </div>
                  <Switch
                    v-model="configuracion.autenticacionDosFactores"
                    class="ml-4"
                  />
                </div>
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Cambiar Contraseña</label>
                  <div class="space-y-3">
                    <input
                      type="password"
                      v-model="configuracion.contraseñaActual"
                      placeholder="Contraseña actual"
                      class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                    <input
                      type="password"
                      v-model="configuracion.nuevaContraseña"
                      placeholder="Nueva contraseña"
                      class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                    <input
                      type="password"
                      v-model="configuracion.confirmarContraseña"
                      placeholder="Confirmar nueva contraseña"
                      class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                    />
                  </div>
                </div>
              </div>
            </div>

            <!-- Personalización -->
            <div v-if="seccionActiva === 'personalizacion'">
              <h2 class="text-lg font-semibold mb-4">Personalización</h2>
              <div class="space-y-4">
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Tema</label>
                  <select
                    v-model="configuracion.tema"
                    class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="claro">Claro</option>
                    <option value="oscuro">Oscuro</option>
                    <option value="sistema">Sistema</option>
                  </select>
                </div>
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Color Principal</label>
                  <div class="flex space-x-2">
                    <button
                      v-for="color in colores"
                      :key="color"
                      class="w-8 h-8 rounded-full border-2"
                      :class="[
                        `bg-${color}-500`,
                        configuracion.colorPrincipal === color ? 'border-gray-900' : 'border-transparent'
                      ]"
                      @click="configuracion.colorPrincipal = color"
                    ></button>
                  </div>
                </div>
                <div>
                  <label class="block text-sm font-medium text-gray-700 mb-1">Densidad de la Interfaz</label>
                  <select
                    v-model="configuracion.densidadInterfaz"
                    class="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500"
                  >
                    <option value="compacta">Compacta</option>
                    <option value="normal">Normal</option>
                    <option value="espaciada">Espaciada</option>
                  </select>
                </div>
              </div>
            </div>

            <!-- Botones de acción -->
            <div class="mt-6 flex justify-end space-x-3">
              <button
                class="px-4 py-2 bg-gray-200 hover:bg-gray-300 rounded-md"
                @click="restaurarConfiguracion"
              >
                Restaurar
              </button>
              <button
                class="px-4 py-2 bg-blue-600 text-white hover:bg-blue-700 rounded-md"
                @click="guardarConfiguracion"
                :disabled="guardando"
              >
                {{ guardando ? 'Guardando...' : 'Guardar Cambios' }}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import {
  Cog6ToothIcon,
  BellIcon,
  ShieldCheckIcon,
  PaintBrushIcon
} from '@heroicons/vue/24/outline'
import { Switch } from '@headlessui/vue'
import { useToast } from 'vue-toastification'

const toast = useToast()
const guardando = ref(false)
const seccionActiva = ref('general')

const secciones = [
  { id: 'general', titulo: 'Ajustes Generales', icono: Cog6ToothIcon },
  { id: 'notificaciones', titulo: 'Notificaciones', icono: BellIcon },
  { id: 'seguridad', titulo: 'Seguridad', icono: ShieldCheckIcon },
  { id: 'personalizacion', titulo: 'Personalización', icono: PaintBrushIcon }
]

const colores = ['blue', 'green', 'purple', 'red', 'yellow']

const configuracion = ref({
  // Ajustes Generales
  nombreEmpresa: '',
  zonaHoraria: 'America/Argentina/Buenos_Aires',
  idioma: 'es',

  // Notificaciones
  notificacionesEmail: true,
  notificacionesPush: true,
  recordatoriosVencimientos: true,

  // Seguridad
  autenticacionDosFactores: false,
  contraseñaActual: '',
  nuevaContraseña: '',
  confirmarContraseña: '',

  // Personalización
  tema: 'claro',
  colorPrincipal: 'blue',
  densidadInterfaz: 'normal'
})

// Cargar configuración inicial
async function cargarConfiguracion() {
  try {
    // TODO: Implementar llamada al API
    // const response = await configuracionService.getConfiguracion()
    // configuracion.value = response.data
  } catch (error) {
    toast.error('Error al cargar la configuración')
    console.error('Error:', error)
  }
}

// Guardar configuración
async function guardarConfiguracion() {
  guardando.value = true
  try {
    // TODO: Implementar llamada al API
    // await configuracionService.guardarConfiguracion(configuracion.value)
    toast.success('Configuración guardada correctamente')
  } catch (error) {
    toast.error('Error al guardar la configuración')
    console.error('Error:', error)
  } finally {
    guardando.value = false
  }
}

// Restaurar configuración
function restaurarConfiguracion() {
  cargarConfiguracion()
  toast.info('Configuración restaurada')
}

// Cargar configuración al montar el componente
cargarConfiguracion()
</script>

<style scoped>
</style> 