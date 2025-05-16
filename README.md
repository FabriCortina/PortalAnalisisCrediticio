# Portal de Análisis Crediticio

Sistema de análisis crediticio que permite evaluar el riesgo de otorgamiento de créditos a clientes, considerando múltiples factores y generando informes detallados. Este sistema está diseñado para ayudar a las instituciones financieras a tomar decisiones informadas sobre la concesión de créditos.

## Características Principales

- Análisis de riesgo crediticio automatizado
- Evaluación de múltiples factores:
  - Historial de pagos
  - Situación financiera
  - Informes externos (Nosis, Veraz, BCRA, AFIP)
  - Garantías ofrecidas
- Generación de informes detallados
- Exportación de informes a PDF
- Integración con servicios externos

## Requisitos

- .NET 7.0 SDK
- PostgreSQL
- Visual Studio 2022 o Visual Studio Code
- Git para la gestión del repositorio
- Acceso a servicios externos (Nosis, Veraz, BCRA, AFIP) para la obtención de informes

## Configuración

1. Clonar el repositorio:
```bash
git clone https://github.com/tu-usuario/PortalAnalisisCrediticio.git
```

2. Restaurar las dependencias:
```bash
dotnet restore
```

3. Configurar la base de datos:
   - Crear una base de datos PostgreSQL
   - Actualizar la cadena de conexión en `appsettings.json`
   - Configurar las variables de entorno necesarias para los servicios externos

4. Ejecutar las migraciones:
```bash
dotnet ef database update
```

5. Iniciar la aplicación:
```bash
dotnet run
```

## Estructura del Proyecto

```
src/
├── Backend/
│   ├── PortalAnalisisCrediticio.API/           # API REST
│   ├── PortalAnalisisCrediticio.Core/          # Entidades y lógica de negocio
│   ├── PortalAnalisisCrediticio.Infrastructure/# Implementaciones y acceso a datos
│   └── PortalAnalisisCrediticio.Shared/        # DTOs y modelos compartidos
```

- **PortalAnalisisCrediticio.API**: Contiene los controladores y la configuración de la API.
- **PortalAnalisisCrediticio.Core**: Define las entidades y la lógica de negocio.
- **PortalAnalisisCrediticio.Infrastructure**: Implementa el acceso a datos y servicios externos.
- **PortalAnalisisCrediticio.Shared**: Contiene DTOs y modelos compartidos entre diferentes capas.

## API Endpoints

### Análisis de Riesgo
- `POST /api/analisisriesgo/realizar/{clienteId}` - Realizar análisis de riesgo
- `GET /api/analisisriesgo/{clienteId}` - Obtener último informe
- `GET /api/analisisriesgo/exportar-pdf/{clienteId}` - Exportar informe a PDF

### Exportación de Datos
- `GET /api/dashboard/exportar/creditos-activos/excel` - Exportar créditos activos a Excel
- `GET /api/dashboard/exportar/creditos-activos/csv` - Exportar créditos activos a CSV
- `GET /api/dashboard/exportar/clientes-riesgo/excel` - Exportar clientes en riesgo a Excel
- `GET /api/dashboard/exportar/clientes-riesgo/csv` - Exportar clientes en riesgo a CSV
- `GET /api/dashboard/exportar/creditos-vencidos/excel` - Exportar créditos vencidos a Excel
- `GET /api/dashboard/exportar/creditos-vencidos/csv` - Exportar créditos vencidos a CSV
- `GET /api/dashboard/exportar/metricas/excel` - Exportar métricas a Excel
- `GET /api/dashboard/exportar/metricas/csv` - Exportar métricas a CSV

#### Ejemplos de Uso
Para exportar créditos activos a Excel, realiza una solicitud GET a:
```
GET /api/dashboard/exportar/creditos-activos/excel?fechaInicio=2023-01-01&fechaFin=2023-12-31
```

Para exportar clientes en riesgo a CSV, realiza una solicitud GET a:
```
GET /api/dashboard/exportar/clientes-riesgo/csv?fechaInicio=2023-01-01&fechaFin=2023-12-31
```

#### Formato de los Archivos Exportados
- **Excel (.xlsx)**: Contiene hojas de cálculo con datos formateados, incluyendo encabezados y estilos.
- **CSV**: Archivos de texto plano con valores separados por comas, ideales para importar en otras aplicaciones.

## Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles. 