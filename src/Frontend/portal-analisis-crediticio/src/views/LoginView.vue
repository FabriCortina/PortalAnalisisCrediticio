<template>
  <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-50 to-gray-100">
    <div class="max-w-md w-full mx-4">
      <div class="bg-white rounded-2xl shadow-xl p-8">
        <!-- Logo o Título -->
        <div class="text-center mb-8">
          <h1 class="text-2xl font-bold text-gray-800">Portal de Análisis Crediticio</h1>
          <p class="text-gray-600 mt-2">Ingresa tus credenciales para continuar</p>
        </div>

        <!-- Formulario -->
        <form @submit.prevent="handleSubmit" class="space-y-6">
          <!-- Email -->
          <div>
            <label for="email" class="block text-sm font-medium text-gray-700">Correo electrónico</label>
            <div class="mt-1 relative rounded-md shadow-sm">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <EnvelopeIcon class="h-5 w-5 text-gray-400" />
              </div>
              <input
                type="email"
                id="email"
                v-model="email"
                :class="[
                  'block w-full pl-10 pr-3 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500',
                  emailError ? 'border-red-300' : 'border-gray-300'
                ]"
                placeholder="ejemplo@correo.com"
                @blur="validateEmail"
              />
            </div>
            <p v-if="emailError" class="mt-1 text-sm text-red-600">{{ emailError }}</p>
          </div>

          <!-- Contraseña -->
          <div>
            <label for="password" class="block text-sm font-medium text-gray-700">Contraseña</label>
            <div class="mt-1 relative rounded-md shadow-sm">
              <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <LockClosedIcon class="h-5 w-5 text-gray-400" />
              </div>
              <input
                :type="showPassword ? 'text' : 'password'"
                id="password"
                v-model="password"
                :class="[
                  'block w-full pl-10 pr-10 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500',
                  passwordError ? 'border-red-300' : 'border-gray-300'
                ]"
                placeholder="••••••••"
                @blur="validatePassword"
              />
              <div class="absolute inset-y-0 right-0 pr-3 flex items-center">
                <button
                  type="button"
                  @click="showPassword = !showPassword"
                  class="text-gray-400 hover:text-gray-500 focus:outline-none"
                >
                  <EyeIcon v-if="!showPassword" class="h-5 w-5" />
                  <EyeOffIcon v-else class="h-5 w-5" />
                </button>
              </div>
            </div>
            <p v-if="passwordError" class="mt-1 text-sm text-red-600">{{ passwordError }}</p>
          </div>

          <!-- Botón de inicio de sesión -->
          <div>
            <button
              type="submit"
              :disabled="isLoading"
              class="w-full flex justify-center py-2 px-4 border border-transparent rounded-lg shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-colors duration-200 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              <template v-if="isLoading">
                <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                  <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                  <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Iniciando sesión...
              </template>
              <template v-else>
                Iniciar sesión
              </template>
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'vue-toastification'
import { EnvelopeIcon, LockClosedIcon, EyeIcon, EyeOffIcon } from '@heroicons/vue/24/outline'

const router = useRouter()
const toast = useToast()

// Estado del formulario
const email = ref('')
const password = ref('')
const showPassword = ref(false)
const isLoading = ref(false)

// Errores de validación
const emailError = ref('')
const passwordError = ref('')

// Validación de email
const validateEmail = () => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!email.value) {
    emailError.value = 'El correo electrónico es requerido'
  } else if (!emailRegex.test(email.value)) {
    emailError.value = 'Ingrese un correo electrónico válido'
  } else {
    emailError.value = ''
  }
}

// Validación de contraseña
const validatePassword = () => {
  if (!password.value) {
    passwordError.value = 'La contraseña es requerida'
  } else if (password.value.length < 6) {
    passwordError.value = 'La contraseña debe tener al menos 6 caracteres'
  } else {
    passwordError.value = ''
  }
}

// Manejo del envío del formulario
const handleSubmit = async () => {
  validateEmail()
  validatePassword()

  if (emailError.value || passwordError.value) {
    return
  }

  isLoading.value = true

  try {
    // Aquí iría la llamada a la API para autenticación
    const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        email: email.value,
        password: password.value,
      }),
    })

    if (!response.ok) {
      throw new Error('Credenciales inválidas')
    }

    const data = await response.json()
    localStorage.setItem('token', data.token)
    router.push('/dashboard')
  } catch (error) {
    toast.error(error.message || 'Error al iniciar sesión')
  } finally {
    isLoading.value = false
  }
}

// Verificar si ya hay un token al cargar la página
onMounted(() => {
  const token = localStorage.getItem('token')
  if (token) {
    router.push('/dashboard')
  }
})
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

body {
  font-family: 'Inter', sans-serif;
}
</style> 