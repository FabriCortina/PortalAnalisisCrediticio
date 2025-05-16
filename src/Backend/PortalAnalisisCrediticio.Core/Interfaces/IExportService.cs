using System.Threading.Tasks;
using System.Collections.Generic;
using PortalAnalisisCrediticio.Shared.DTOs.Dashboard;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    /// <summary>
    /// Servicio para la exportación de datos en diferentes formatos
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Exporta datos a Excel
        /// </summary>
        /// <param name="datos">Datos a exportar</param>
        /// <param name="nombreHoja">Nombre de la hoja de Excel</param>
        /// <returns>Archivo Excel en bytes</returns>
        Task<byte[]> ExportarExcelAsync<T>(IEnumerable<T> datos, string nombreHoja);

        /// <summary>
        /// Exporta datos a CSV
        /// </summary>
        /// <param name="datos">Datos a exportar</param>
        /// <returns>Archivo CSV en bytes</returns>
        Task<byte[]> ExportarCSVAsync<T>(IEnumerable<T> datos);

        /// <summary>
        /// Genera un informe en PDF
        /// </summary>
        /// <param name="datos">Datos del informe</param>
        /// <param name="plantilla">Plantilla HTML a utilizar</param>
        /// <returns>Archivo PDF en bytes</returns>
        Task<byte[]> GenerarPDFAsync<T>(T datos, string plantilla);

        /// <summary>
        /// Exporta un gráfico a imagen
        /// </summary>
        /// <param name="datos">Datos del gráfico</param>
        /// <param name="tipoGrafico">Tipo de gráfico (Pie, Bar, Line)</param>
        /// <returns>Imagen en bytes</returns>
        Task<byte[]> ExportarGraficoAsync<T>(T datos, string tipoGrafico);

        Task<byte[]> ExportarCreditosActivosToExcelAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> ExportarCreditosActivosToCsvAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> ExportarClientesRiesgoToExcelAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> ExportarClientesRiesgoToCsvAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> ExportarCreditosVencidosToExcelAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> ExportarCreditosVencidosToCsvAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> ExportarMetricasToExcelAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<byte[]> ExportarMetricasToCsvAsync(DateTime fechaInicio, DateTime fechaFin);
    }
} 