<template>
  <div class="flex min-h-0 flex-1 flex-col bg-white">
    <!-- Logo -->
    <div class="flex h-16 flex-shrink-0 items-center px-4">
      <h1 class="text-xl font-bold text-gray-900">Portal Crediticio</h1>
    </div>

    <!-- Navegación -->
    <nav class="flex-1 space-y-1 px-2 py-4">
      <router-link
        v-for="item in navigation"
        :key="item.name"
        :to="item.to"
        :class="[
          isCurrentRoute(item.to)
            ? 'bg-gray-100 text-gray-900'
            : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900',
          'group flex items-center px-2 py-2 text-sm font-medium rounded-md'
        ]"
      >
        <component
          :is="item.icon"
          :class="[
            isCurrentRoute(item.to)
              ? 'text-gray-500'
              : 'text-gray-400 group-hover:text-gray-500',
            'mr-3 flex-shrink-0 h-6 w-6'
          ]"
        />
        {{ item.name }}
      </router-link>
    </nav>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import {
  HomeIcon,
  ChartBarIcon,
  UserGroupIcon,
  DocumentTextIcon,
  BellIcon,
  Cog6ToothIcon
} from '@heroicons/vue/24/outline'

const route = useRoute()

const navigation = [
  { name: 'Dashboard', to: '/dashboard', icon: HomeIcon },
  { name: 'Análisis', to: '/analisis', icon: ChartBarIcon },
  { name: 'Clientes', to: '/clientes', icon: UserGroupIcon },
  { name: 'Reportes', to: '/reportes', icon: DocumentTextIcon },
  { name: 'Alertas', to: '/alertas', icon: BellIcon },
  { name: 'Configuración', to: '/configuracion', icon: Cog6ToothIcon }
]

const isCurrentRoute = (path) => {
  return route.path.startsWith(path)
}
</script> 