# Portal de Análisis Crediticio

Sistema integral para la gestión y análisis de créditos, con enfoque en la evaluación de riesgos y la experiencia de usuario.

## 🚀 Características Principales

### 1. Arquitectura y Estructura
- **Frontend**: Vue 3 + Vite
- **Backend**: .NET Core
- **Base de Datos**: SQL Server
- **Autenticación**: JWT
- **Estilos**: Tailwind CSS

### 2. Optimizaciones de Rendimiento

#### 2.1 Lazy Loading de Imágenes
```vue
<LazyImage
  :src="imagen"
  :alt="descripcion"
  :aspect-ratio="100"
  rounded
/>
```
- Componente reutilizable para carga diferida de imágenes
- Placeholder durante la carga
- Soporte para diferentes aspect ratios
- Optimización de rendimiento en listas largas

#### 2.2 Carga de Componentes
```javascript
component: () => import(/* webpackChunkName: "dashboard" */ '@/views/DashboardView.vue')
```
- Lazy loading de rutas con webpack chunks
- Carga bajo demanda de componentes
- Mejor tiempo de carga inicial
- Optimización de recursos

#### 2.3 Sistema de Caché
```javascript
async getKPIs() {
  return cacheService.getOrSet('dashboard_kpis', async () => {
    // Lógica de obtención de datos
  }, 5 * 60 * 1000)
}
```
- Caché en memoria con TTL configurable
- Limpieza automática de datos expirados
- Optimización de peticiones al servidor
- Gestión centralizada del caché

### 3. Experiencia de Usuario

#### 3.1 Diseño Responsive
- Layout adaptativo para móvil y desktop
- Sidebar colapsable en móvil
- Grid system flexible
- Componentes adaptables

#### 3.2 Feedback Visual
- Toasts para notificaciones
- Skeletons durante la carga
- Estados de hover y focus
- Modales para confirmaciones
- Indicadores de progreso

#### 3.3 Validación en Tiempo Real
- Validación de formularios
- Mensajes de error contextuales
- Debounce en búsquedas
- Feedback inmediato

### 4. Seguridad

#### 4.1 Autenticación
- JWT para gestión de sesiones
- Protección de rutas
- Refresh tokens
- Manejo de roles y permisos

#### 4.2 Protección de Datos
- Sanitización de inputs
- Validación de datos
- Cifrado de información sensible
- Headers de seguridad

### 5. Funcionalidades Principales

#### 5.1 Dashboard
- KPIs en tiempo real
- Gráficos interactivos
- Filtros dinámicos
- Exportación de datos

#### 5.2 Gestión de Clientes
- CRUD completo
- Historial de cambios
- Documentación digital
- Análisis de riesgo

#### 5.3 Análisis Crediticio
- Cálculo de riesgo
- Generación de informes
- Historial de análisis
- Recomendaciones automáticas

#### 5.4 Sistema de Logs
- Registro de actividades
- Filtros avanzados
- Exportación de logs
- Auditoría de cambios

### 6. Buenas Prácticas

#### 6.1 Código
- Componentes reutilizables
- Composición sobre herencia
- Manejo de errores consistente
- Documentación de código
- Tests unitarios

#### 6.2 Estado
- Gestión centralizada
- Reactividad eficiente
- Caché inteligente
- Persistencia de datos

#### 6.3 API
- RESTful
- Versionado
- Documentación con Swagger
- Rate limiting
- Caché HTTP

### 7. Optimizaciones

#### 7.1 Frontend
- Code splitting
- Tree shaking
- Minificación
- Compresión de assets
- Preload de recursos críticos

#### 7.2 Backend
- Caché en múltiples niveles
- Compresión de respuestas
- Paginación eficiente
- Queries optimizadas

### 8. Monitoreo y Logging

#### 8.1 Frontend
- Error tracking
- Performance monitoring
- User analytics
- Console logging

#### 8.2 Backend
- Application insights
- Log aggregation
- Performance metrics
- Health checks

## 🛠️ Instalación

1. Clonar el repositorio
```bash
git clone https://github.com/tu-usuario/portal-analisis-crediticio.git
```

2. Instalar dependencias Frontend
```bash
cd src/Frontend/portal-analisis-crediticio
npm install
```

3. Instalar dependencias Backend
```bash
cd src/Backend
dotnet restore
```

4. Configurar variables de entorno
```bash
# Frontend
cp .env.example .env

# Backend
cp appsettings.Example.json appsettings.json
```

5. Iniciar desarrollo
```bash
# Frontend
npm run dev

# Backend
dotnet run
```

## 📦 Despliegue

### Frontend
```bash
npm run build
```

### Backend
```bash
dotnet publish -c Release
```

## 🧪 Pruebas Funcionales

### 1. Autenticación y Autorización

#### 1.1 Login
- [ ] Inicio de sesión exitoso con credenciales válidas
- [ ] Validación de campos requeridos
- [ ] Mensaje de error con credenciales inválidas
- [ ] Bloqueo después de múltiples intentos fallidos
- [ ] Redirección a página anterior después del login

#### 1.2 Gestión de Sesión
- [ ] Expiración de token JWT
- [ ] Renovación automática de token
- [ ] Cierre de sesión exitoso
- [ ] Persistencia de sesión en refresh
- [ ] Bloqueo de acceso a rutas protegidas

#### 1.3 Roles y Permisos
- [ ] Acceso según nivel de rol (Admin, Analista, Usuario)
- [ ] Restricción de funcionalidades por rol
- [ ] Visualización de menús según permisos
- [ ] Validación de acciones según permisos

### 2. Gestión de Clientes

#### 2.1 Registro de Clientes
- [ ] Creación exitosa de nuevo cliente
- [ ] Validación de campos obligatorios
- [ ] Validación de formato de datos (CUIT, email, teléfono)
- [ ] Carga de documentación
- [ ] Mensajes de éxito/error

#### 2.2 Edición de Clientes
- [ ] Modificación de datos básicos
- [ ] Actualización de información financiera
- [ ] Historial de cambios
- [ ] Validación de campos modificados
- [ ] Notificación de cambios

#### 2.3 Búsqueda y Filtros
- [ ] Búsqueda por nombre/CUIT
- [ ] Filtros avanzados
- [ ] Ordenamiento de resultados
- [ ] Paginación
- [ ] Exportación de resultados

### 3. Análisis de Riesgo

#### 3.1 Cálculo de Riesgo
- [ ] Ejecución de análisis completo
- [ ] Validación de datos requeridos
- [ ] Cálculo correcto de score
- [ ] Asignación de nivel de riesgo
- [ ] Generación de recomendaciones

#### 3.2 Informes
- [ ] Generación de informe detallado
- [ ] Visualización en web
- [ ] Exportación a PDF
- [ ] Historial de informes
- [ ] Comparación de informes

#### 3.3 Integración con Servicios
- [ ] Consulta a Nosis
- [ ] Consulta a Veraz
- [ ] Consulta a BCRA
- [ ] Consulta a AFIP
- [ ] Manejo de errores de servicios

### 4. Dashboard y Reportes

#### 4.1 KPIs
- [ ] Actualización en tiempo real
- [ ] Cálculo correcto de métricas
- [ ] Filtros por período
- [ ] Exportación de datos
- [ ] Gráficos interactivos

#### 4.2 Alertas
- [ ] Generación de alertas
- [ ] Notificaciones en tiempo real
- [ ] Filtros de alertas
- [ ] Marcado como leído
- [ ] Acciones sobre alertas

### 5. Exportación de Datos

#### 5.1 Formatos
- [ ] Exportación a Excel
- [ ] Exportación a CSV
- [ ] Exportación a PDF
- [ ] Validación de datos exportados
- [ ] Nombres de archivo correctos

#### 5.2 Filtros de Exportación
- [ ] Selección de campos
- [ ] Filtros por fecha
- [ ] Filtros por tipo
- [ ] Límites de registros
- [ ] Formato de datos

### 6. Rendimiento y Optimización

#### 6.1 Carga de Páginas
- [ ] Tiempo de carga inicial
- [ ] Carga de componentes
- [ ] Lazy loading de imágenes
- [ ] Caché de datos
- [ ] Optimización de recursos

#### 6.2 Operaciones
- [ ] Tiempo de respuesta de API
- [ ] Procesamiento de datos grandes
- [ ] Manejo de concurrencia
- [ ] Uso de memoria
- [ ] Optimización de queries

### 7. Usabilidad

#### 7.1 Navegación
- [ ] Menú responsive
- [ ] Breadcrumbs
- [ ] Navegación intuitiva
- [ ] Accesos rápidos
- [ ] Búsqueda global

#### 7.2 Formularios
- [ ] Validación en tiempo real
- [ ] Mensajes de error claros
- [ ] Autocompletado
- [ ] Guardado automático
- [ ] Navegación entre campos

### 8. Compatibilidad

#### 8.1 Navegadores
- [ ] Chrome (últimas 2 versiones)
- [ ] Firefox (últimas 2 versiones)
- [ ] Safari (últimas 2 versiones)
- [ ] Edge (últimas 2 versiones)
- [ ] Opera (última versión)

#### 8.2 Dispositivos
- [ ] Desktop (1920x1080, 1366x768)
- [ ] Tablet (768x1024, 1024x768)
- [ ] Móvil (375x667, 414x896)
- [ ] Orientación vertical/horizontal
- [ ] Touch/click

### 9. Seguridad

#### 9.1 Datos Sensibles
- [ ] Cifrado de datos
- [ ] Protección de información
- [ ] Sanitización de inputs
- [ ] Validación de datos
- [ ] Headers de seguridad

#### 9.2 Auditoría
- [ ] Registro de acciones
- [ ] Trazabilidad de cambios
- [ ] Exportación de logs
- [ ] Filtros de auditoría
- [ ] Retención de logs

### 10. Recuperación

#### 10.1 Errores
- [ ] Mensajes de error claros
- [ ] Recuperación de sesión
- [ ] Guardado automático
- [ ] Restauración de datos
- [ ] Logs de error

#### 10.2 Conectividad
- [ ] Manejo de desconexión
- [ ] Reconexión automática
- [ ] Sincronización de datos
- [ ] Estado offline
- [ ] Recuperación de operaciones

## 🧪 Testing

### Frontend
```bash
npm run test:unit
npm run test:e2e
```

### Backend
```bash
dotnet test
```

## 📚 Documentación Adicional

- [Guía de Contribución](CONTRIBUTING.md)
- [Guía de Estilo](STYLE_GUIDE.md)
- [API Documentation](API.md)
- [Arquitectura](ARCHITECTURE.md)

## 📚 Documentación de API

### Swagger UI

La documentación completa de la API está disponible a través de Swagger UI en:
```
https://[tu-dominio]/swagger
```

### Endpoints Principales

#### Autenticación
```http
POST /api/auth/login
POST /api/auth/refresh
POST /api/auth/logout
```

#### Clientes
```http
GET    /api/clientes
POST   /api/clientes
GET    /api/clientes/{id}
PUT    /api/clientes/{id}
DELETE /api/clientes/{id}
GET    /api/clientes/{id}/historial
```

#### Análisis de Riesgo
```http
POST   /api/analisis-riesgo
GET    /api/analisis-riesgo/{id}
GET    /api/analisis-riesgo/cliente/{clienteId}
GET    /api/analisis-riesgo/{id}/informe
GET    /api/analisis-riesgo/{id}/informe/pdf
```

#### Dashboard
```http
GET /api/dashboard/kpis
GET /api/dashboard/distribucion-riesgos
GET /api/dashboard/solicitudes-mes
GET /api/dashboard/alertas
```

#### Exportación
```http
GET /api/exportar/clientes
GET /api/exportar/analisis
GET /api/exportar/informes
GET /api/exportar/logs
```

### Modelos de Datos

#### Cliente
```json
{
  "id": "integer",
  "cuit": "string",
  "razonSocial": "string",
  "tipoPersona": "string",
  "direccion": {
    "calle": "string",
    "numero": "string",
    "localidad": "string",
    "provincia": "string",
    "codigoPostal": "string"
  },
  "contacto": {
    "email": "string",
    "telefono": "string"
  },
  "informacionFinanciera": {
    "ingresosMensuales": "number",
    "gastosMensuales": "number",
    "activos": "number",
    "pasivos": "number"
  }
}
```

#### Análisis de Riesgo
```json
{
  "id": "integer",
  "clienteId": "integer",
  "fechaAnalisis": "datetime",
  "score": "number",
  "nivelRiesgo": "string",
  "factores": [
    {
      "nombre": "string",
      "valor": "number",
      "peso": "number"
    }
  ],
  "recomendaciones": [
    {
      "tipo": "string",
      "descripcion": "string",
      "prioridad": "string"
    }
  ]
}
```

#### Informe
```json
{
  "id": "integer",
  "analisisId": "integer",
  "fechaGeneracion": "datetime",
  "contenido": "string",
  "resumen": "string",
  "recomendaciones": [
    {
      "tipo": "string",
      "descripcion": "string",
      "acciones": ["string"]
    }
  ]
}
```

### Códigos de Estado

- `200 OK`: Solicitud exitosa
- `201 Created`: Recurso creado
- `400 Bad Request`: Error en la solicitud
- `401 Unauthorized`: No autenticado
- `403 Forbidden`: No autorizado
- `404 Not Found`: Recurso no encontrado
- `500 Internal Server Error`: Error del servidor

### Autenticación

Todas las peticiones a la API (excepto login) requieren un token JWT en el header:

```http
Authorization: Bearer {token}
```

### Rate Limiting

- 100 peticiones por minuto por IP
- 1000 peticiones por hora por usuario

### Versiones

La API utiliza versionado en la URL:
```
/api/v1/[endpoint]
```

### Ejemplos de Uso

#### Crear Cliente
```http
POST /api/v1/clientes
Content-Type: application/json
Authorization: Bearer {token}

{
  "cuit": "30-12345678-9",
  "razonSocial": "Empresa Ejemplo S.A.",
  "tipoPersona": "JURIDICA",
  "direccion": {
    "calle": "Av. Siempreviva",
    "numero": "742",
    "localidad": "Springfield",
    "provincia": "Buenos Aires",
    "codigoPostal": "1234"
  }
}
```

#### Realizar Análisis de Riesgo
```http
POST /api/v1/analisis-riesgo
Content-Type: application/json
Authorization: Bearer {token}

{
  "clienteId": 123,
  "tipoAnalisis": "COMPLETO",
  "incluirInformesExternos": true
}
```

### Errores

Los errores siguen el formato:
```json
{
  "error": {
    "codigo": "string",
    "mensaje": "string",
    "detalles": ["string"]
  }
}
```

### Paginación

Los endpoints que devuelven listas soportan paginación:
```http
GET /api/v1/clientes?page=1&size=10&sort=razonSocial,asc
```

Respuesta:
```json
{
  "content": [],
  "pageable": {
    "pageNumber": 0,
    "pageSize": 10,
    "sort": {
      "sorted": true,
      "unsorted": false
    }
  },
  "totalElements": 100,
  "totalPages": 10,
  "last": false,
  "first": true,
  "empty": false
}
```

### Filtros

Los endpoints soportan filtros avanzados:
```http
GET /api/v1/clientes?filtros[nivelRiesgo]=ALTO&filtros[fechaDesde]=2024-01-01
```

### Ordenamiento

Soporte para ordenamiento múltiple:
```http
GET /api/v1/clientes?sort=razonSocial,asc&sort=fechaCreacion,desc
```

## 🤝 Contribución

1. Fork el proyecto
2. Crear rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE.md](LICENSE.md) para más detalles.

## 📞 Soporte

Para soporte, email soporte@portal-analisis-crediticio.com o crear un issue en el repositorio. 