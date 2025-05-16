<template>
  <div class="min-h-screen bg-gray-100">
    <!-- Sidebar para móvil -->
    <div
      v-if="isSidebarOpen"
      class="fixed inset-0 z-40 lg:hidden"
      @click="toggleSidebar"
    >
      <div class="fixed inset-0 bg-gray-600 bg-opacity-75"></div>
      <div class="fixed inset-y-0 left-0 flex w-64 flex-col bg-white">
        <div class="flex h-16 items-center justify-between px-4">
          <h1 class="text-xl font-bold text-gray-800">Portal Análisis</h1>
          <button
            class="text-gray-500 hover:text-gray-600"
            @click="toggleSidebar"
          >
            <XMarkIcon class="h-6 w-6" />
          </button>
        </div>
        <SidebarContent />
      </div>
    </div>

    <!-- Sidebar para desktop -->
    <div class="hidden lg:fixed lg:inset-y-0 lg:flex lg:w-64 lg:flex-col">
      <div class="flex min-h-0 flex-1 flex-col bg-white">
        <div class="flex h-16 items-center px-4">
          <h1 class="text-xl font-bold text-gray-800">Portal Análisis</h1>
        </div>
        <SidebarContent />
      </div>
    </div>

    <!-- Contenido principal -->
    <div class="lg:pl-64">
      <!-- Header -->
      <div class="sticky top-0 z-10 flex h-16 flex-shrink-0 bg-white shadow">
        <button
          type="button"
          class="px-4 text-gray-500 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-blue-500 lg:hidden"
          @click="toggleSidebar"
        >
          <Bars3Icon class="h-6 w-6" />
        </button>

        <!-- Perfil de usuario -->
        <div class="flex flex-1 justify-end px-4">
          <div class="ml-4 flex items-center md:ml-6">
            <div class="relative">
              <button
                type="button"
                class="flex max-w-xs items-center rounded-full bg-white text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
                @click="isProfileOpen = !isProfileOpen"
              >
                <span class="sr-only">Abrir menú de usuario</span>
                <img
                  class="h-8 w-8 rounded-full"
                  :src="authStore.user?.avatar || 'https://ui-avatars.com/api/?name=' + authStore.userName"
                  :alt="authStore.userName"
                />
              </button>

              <!-- Menú desplegable del perfil -->
              <div
                v-if="isProfileOpen"
                class="absolute right-0 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
                role="menu"
                aria-orientation="vertical"
                aria-labelledby="user-menu-button"
                tabindex="-1"
              >
                <div class="px-4 py-2 text-sm text-gray-700">
                  <p class="font-medium">{{ authStore.userName }}</p>
                  <p class="text-gray-500">{{ authStore.user?.email }}</p>
                </div>
                <div class="border-t border-gray-100"></div>
                <a
                  href="#"
                  class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                  role="menuitem"
                  tabindex="-1"
                  @click="router.push('/perfil')"
                >
                  Tu Perfil
                </a>
                <a
                  href="#"
                  class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                  role="menuitem"
                  tabindex="-1"
                  @click="router.push('/configuracion')"
                >
                  Configuración
                </a>
                <div class="border-t border-gray-100"></div>
                <button
                  class="block w-full px-4 py-2 text-left text-sm text-gray-700 hover:bg-gray-100"
                  role="menuitem"
                  tabindex="-1"
                  @click="handleLogout"
                >
                  Cerrar Sesión
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Contenido de la página -->
      <main class="py-6">
        <div class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
          <slot></slot>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { useToast } from 'vue-toastification'
import { Bars3Icon, XMarkIcon } from '@heroicons/vue/24/outline'
import SidebarContent from '@/components/SidebarContent.vue'

const router = useRouter()
const toast = useToast()
const authStore = useAuthStore()
const isSidebarOpen = ref(false)
const isProfileOpen = ref(false)

// Verificar autenticación al montar el componente
onMounted(async () => {
  if (!authStore.isAuthenticated) {
    try {
      await authStore.fetchUser()
    } catch (error) {
      router.push('/login')
    }
  }
})

// Función para cerrar sesión
async function handleLogout() {
  try {
    await authStore.logout()
    router.push('/login')
  } catch (error) {
    console.error('Error al cerrar sesión:', error)
  }
}

// Función para alternar el sidebar
function toggleSidebar() {
  isSidebarOpen.value = !isSidebarOpen.value
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style> 