import './assets/main.css'
import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Toast from 'vue-toastification'
import App from './App.vue'
import router from './router'
import { useAuthStore } from './stores/authStore'
import VueApexCharts from 'vue3-apexcharts'

// Configuración de Toast
const toastOptions = {
  position: 'top-right',
  timeout: 3000,
  closeOnClick: true,
  pauseOnFocusLoss: true,
  pauseOnHover: true,
  draggable: true,
  draggablePercent: 0.6,
  showCloseButtonOnHover: false,
  hideProgressBar: false,
  closeButton: 'button',
  icon: true,
  rtl: false
}

const app = createApp(App)

// Configuración de ApexCharts
app.use(VueApexCharts)

// Configurar Pinia
const pinia = createPinia()
app.use(pinia)

// Inicializar el store de autenticación
const authStore = useAuthStore()
authStore.initialize()

// Configurar el router
app.use(router)

// Configurar Toast
app.use(Toast, toastOptions)

app.mount('#app')
