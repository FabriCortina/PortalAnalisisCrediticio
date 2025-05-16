using System.Threading.Tasks;
using System.Collections.Generic;
using PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface IExportService
    {
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