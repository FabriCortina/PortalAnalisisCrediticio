using System;
using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que contiene toda la información del dashboard
/// </summary>
public class DashboardDTO
{
    /// <summary>
    /// KPIs principales del dashboard
    /// </summary>
    public KPIsDTO KPIs { get; set; }

    /// <summary>
    /// Distribución de clientes por nivel de riesgo
    /// </summary>
    public DistribucionRiesgosDTO DistribucionRiesgos { get; set; }

    /// <summary>
    /// Distribución de solicitudes por mes
    /// </summary>
    public SolicitudesPorMesDTO SolicitudesPorMes { get; set; }

    /// <summary>
    /// Lista de alertas activas
    /// </summary>
    public IEnumerable<AlertaDTO> Alertas { get; set; }

    /// <summary>
    /// Lista de créditos activos
    /// </summary>
    public List<CreditoActivoDTO> CreditosActivos { get; set; }

    /// <summary>
    /// Lista de clientes en riesgo
    /// </summary>
    public List<ClienteRiesgoDTO> ClientesRiesgo { get; set; }

    /// <summary>
    /// Lista de créditos vencidos
    /// </summary>
    public List<CreditoVencidoDTO> CreditosVencidos { get; set; }

    /// <summary>
    /// Métricas generales del sistema
    /// </summary>
    public MetricasDTO Metricas { get; set; }

    /// <summary>
    /// Actividades recientes del sistema
    /// </summary>
    public List<ActividadDTO> ActividadesRecientes { get; set; }
}