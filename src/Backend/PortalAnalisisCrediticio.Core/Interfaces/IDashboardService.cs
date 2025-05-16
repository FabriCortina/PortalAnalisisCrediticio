using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> ObtenerDashboardAsync();
        Task<List<CreditoActivoDTO>> ObtenerCreditosActivosAsync();
        Task<List<ClienteRiesgoDTO>> ObtenerClientesRiesgoAsync();
        Task<List<CreditoVencidoDTO>> ObtenerCreditosVencidosAsync();
        Task<MetricasDTO> ObtenerMetricasAsync();
        Task<List<ActividadDTO>> ObtenerActividadesRecientesAsync();
    }
} 