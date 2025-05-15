# Portal de Análisis Crediticio

Sistema de análisis crediticio que permite evaluar el riesgo de otorgamiento de créditos a clientes, considerando múltiples factores y generando informes detallados.

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

## API Endpoints

### Análisis de Riesgo
- `POST /api/analisisriesgo/realizar/{clienteId}` - Realizar análisis de riesgo
- `GET /api/analisisriesgo/{clienteId}` - Obtener último informe
- `GET /api/analisisriesgo/exportar-pdf/{clienteId}` - Exportar informe a PDF

## Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles. 