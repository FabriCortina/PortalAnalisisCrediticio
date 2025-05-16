import { createRouter, createWebHistory } from 'vue-router'
import { authGuard, guestGuard } from './guards'

const routes = [
  {
    path: '/',
    component: () => import('@/layouts/AppLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      {
        path: '',
        name: 'home',
        component: () => import('@/views/HomeView.vue')
      },
      {
        path: 'clientes',
        name: 'clientes',
        component: () => import(/* webpackChunkName: "clientes" */ '@/views/ClientesView.vue'),
        meta: {
          requiresAuth: true,
          title: 'Clientes'
        }
      },
      {
        path: 'clientes/nuevo',
        name: 'nuevo-cliente',
        component: () => import('@/views/ClienteFormView.vue')
      },
      {
        path: 'clientes/:id',
        name: 'editar-cliente',
        component: () => import('@/views/ClienteFormView.vue')
      },
      {
        path: 'creditos',
        name: 'creditos',
        component: () => import('@/views/CreditosActivosView.vue'),
        meta: {
          requiresAuth: true,
          title: 'Créditos Activos'
        }
      },
      {
        path: 'reportes',
        name: 'reportes',
        component: () => import('@/views/ReportesView.vue')
      },
      {
        path: 'alertas',
        name: 'alertas',
        component: () => import('@/views/AlertasView.vue')
      },
      {
        path: 'configuracion',
        name: 'configuracion',
        component: () => import('@/views/ConfiguracionView.vue')
      },
      {
        path: 'perfil',
        name: 'perfil',
        component: () => import('@/views/PerfilView.vue')
      },
      {
        path: 'informes',
        name: 'Informes',
        component: () => import('@/views/InformesView.vue'),
        meta: {
          requiresAuth: true,
          title: 'Informes'
        }
      },
      {
        path: '/',
        name: 'dashboard',
        component: () => import(/* webpackChunkName: "dashboard" */ '@/views/DashboardView.vue'),
        meta: {
          requiresAuth: true,
          title: 'Dashboard'
        }
      },
      {
        path: 'analisis',
        name: 'analisis',
        component: () => import(/* webpackChunkName: "analisis" */ '@/views/AnalisisView.vue'),
        meta: {
          requiresAuth: true,
          title: 'Análisis'
        }
      },
      {
        path: '/logs',
        name: 'logs',
        component: () => import(/* webpackChunkName: "logs" */ '@/views/LogsView.vue'),
        meta: {
          requiresAuth: true,
          title: 'Logs del Sistema'
        }
      }
    ]
  },
  {
    path: '/login',
    name: 'login',
    component: () => import(/* webpackChunkName: "auth" */ '@/views/LoginView.vue'),
    meta: { guest: true }
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'not-found',
    component: () => import(/* webpackChunkName: "error" */ '@/views/NotFoundView.vue')
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Aplicar guards globalmente
router.beforeEach(authGuard)
router.beforeEach(guestGuard)

export default router 