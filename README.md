# Portal de Análisis Crediticio

Sistema integral para análisis crediticio, evaluación financiera y gestión de clientes en empresas de maquinaria y repuestos agrícolas.

## Características Principales

- Análisis crediticio y evaluación financiera
- Gestión de clientes
- Multiidioma
- Multimoneda
- Multicompañía
- Roles y permisos
- Interfaz responsive
- Generación de informes PDF
- Soporte para normativas fiscales internacionales

## Stack Tecnológico

### Backend
- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL

### Frontend
- Vue.js 3
- Vuetify
- Vuex
- Vue Router
- i18n

## Requisitos Previos

- .NET SDK 8.0 o superior
- Node.js 18.x o superior
- PostgreSQL 15.x o superior
- Visual Studio 2022 o Visual Studio Code

## Estructura del Proyecto

```
PortalAnalisisCrediticio/
├── src/
│   ├── Backend/
│   │   ├── PortalAnalisisCrediticio.API/           # API REST principal
│   │   ├── PortalAnalisisCrediticio.Core/          # Lógica de negocio
│   │   ├── PortalAnalisisCrediticio.Infrastructure/# Implementaciones técnicas
│   │   └── PortalAnalisisCrediticio.Shared/        # DTOs y utilidades compartidas
│   │
│   └── Frontend/
│       ├── portal-analisis-crediticio/             # Aplicación Vue.js
│       └── portal-analisis-crediticio-admin/       # Panel de administración
│
├── tests/
│   ├── Backend.Tests/
│   └── Frontend.Tests/
│
└── docs/
    ├── api/
    └── architecture/
```

## Configuración del Entorno de Desarrollo

1. Clonar el repositorio
```bash
git clone [URL_DEL_REPOSITORIO]
cd PortalAnalisisCrediticio
```

2. Configurar el Backend
```bash
cd src/Backend
dotnet restore
dotnet build
```

3. Configurar el Frontend
```bash
cd src/Frontend/portal-analisis-crediticio
npm install
```

4. Configurar la Base de Datos
- Crear una base de datos PostgreSQL
- Actualizar la cadena de conexión en `appsettings.json`

## Licencia

[Especificar la licencia del proyecto]

## Contacto

[Información de contacto del equipo] 