import { ref } from 'vue'
import { useRouter } from 'vue-router'
import {
  HomeIcon,
  UserGroupIcon,
  DocumentReportIcon,
  CogIcon,
  ChartBarIcon,
  DocumentTextIcon,
  ClipboardCheckIcon,
  CreditCardIcon,
  ClipboardListIcon
} from '@heroicons/vue/outline'

const navigation = [
  {
    name: 'Inicio',
    to: '/',
    icon: HomeIcon
  },
  {
    name: 'Clientes',
    to: '/clientes',
    icon: UserGroupIcon
  },
  {
    name: 'Créditos',
    to: '/creditos',
    icon: CreditCardIcon
  },
  {
    name: 'Informes',
    to: '/informes',
    icon: DocumentReportIcon
  },
  {
    name: 'Reportes',
    to: '/reportes',
    icon: ChartBarIcon
  },
  {
    name: 'Alertas',
    to: '/alertas',
    icon: ClipboardCheckIcon
  },
  {
    name: 'Logs',
    path: '/logs',
    icon: ClipboardListIcon
  },
  {
    name: 'Configuración',
    to: '/configuracion',
    icon: CogIcon
  }
] 