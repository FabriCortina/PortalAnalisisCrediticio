namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que contiene los KPIs principales del dashboard
/// </summary>
public class KPIsDTO
{
    /// <summary>
    /// Total de clientes activos en el sistema
    /// </summary>
    public int TotalClientes { get; set; }

    /// <summary>
    /// Total de créditos activos
    /// </summary>
    public int TotalCreditos { get; set; }

    /// <summary>
    /// Monto total de créditos otorgados
    /// </summary>
    public decimal MontoTotalCreditos { get; set; }

    /// <summary>
    /// Porcentaje de clientes en riesgo
    /// </summary>
    public decimal PorcentajeClientesRiesgo { get; set; }

    /// <summary>
    /// Tasa de morosidad del sistema
    /// </summary>
    public decimal TasaMorosidad { get; set; }

    /// <summary>
    /// Total de solicitudes pendientes
    /// </summary>
    public int SolicitudesPendientes { get; set; }

    /// <summary>
    /// Total de alertas activas
    /// </summary>
    public int TotalAlertas { get; set; }
} 