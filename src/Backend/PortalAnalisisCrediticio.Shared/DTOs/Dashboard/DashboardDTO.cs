using System;
using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard
{
    public class DashboardDTO
    {
        public MetricasDTO Metricas { get; set; }
        public List<CreditoActivoDTO> CreditosActivos { get; set; }
        public List<ClienteRiesgoDTO> ClientesRiesgo { get; set; }
        public List<CreditoVencidoDTO> CreditosVencidos { get; set; }
        public List<ActividadDTO> ActividadesRecientes { get; set; }
    }
}