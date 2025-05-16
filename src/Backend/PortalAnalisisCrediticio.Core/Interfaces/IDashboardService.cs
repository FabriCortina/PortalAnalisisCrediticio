using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    /// <summary>
    /// Servicio para la gestión del dashboard y métricas
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Obtiene los KPIs principales del dashboard
        /// </summary>
        /// <returns>KPIs actualizados</returns>
        Task<KPIsDTO> GetKPIsAsync();

        /// <summary>
        /// Obtiene la distribución de riesgos
        /// </summary>
        /// <returns>Distribución de clientes por nivel de riesgo</returns>
        Task<DistribucionRiesgosDTO> GetDistribucionRiesgosAsync();

        /// <summary>
        /// Obtiene las solicitudes por mes
        /// </summary>
        /// <param name="año">Año a consultar</param>
        /// <returns>Distribución de solicitudes por mes</returns>
        Task<SolicitudesPorMesDTO> GetSolicitudesPorMesAsync(int año);

        /// <summary>
        /// Obtiene las alertas activas
        /// </summary>
        /// <returns>Lista de alertas</returns>
        Task<IEnumerable<AlertaDTO>> GetAlertasAsync();

        /// <summary>
        /// Exporta los datos del dashboard
        /// </summary>
        /// <param name="tipo">Tipo de exportación (Excel/CSV)</param>
        /// <param name="filtros">Filtros opcionales</param>
        /// <returns>Archivo con los datos exportados</returns>
        Task<byte[]> ExportarDatosAsync(string tipo, Dictionary<string, string> filtros = null);

        Task<DashboardDTO> ObtenerDashboardAsync();
        Task<List<CreditoActivoDTO>> ObtenerCreditosActivosAsync();
        Task<List<ClienteRiesgoDTO>> ObtenerClientesRiesgoAsync();
        Task<List<CreditoVencidoDTO>> ObtenerCreditosVencidosAsync();
        Task<MetricasDTO> ObtenerMetricasAsync();
        Task<List<ActividadDTO>> ObtenerActividadesRecientesAsync();
        Task<DashboardAdminDTO> ObtenerDashboardAdminAsync();
    }
} 