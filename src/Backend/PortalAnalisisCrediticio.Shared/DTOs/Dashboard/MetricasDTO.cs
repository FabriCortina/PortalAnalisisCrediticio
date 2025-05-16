using System;
using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que contiene las métricas generales del sistema
/// </summary>
public class MetricasDTO
{
    /// <summary>
    /// Tasa de morosidad general
    /// </summary>
    public decimal TasaMorosidad { get; set; }

    /// <summary>
    /// Tasa de aprobación de créditos
    /// </summary>
    public decimal TasaAprobacion { get; set; }

    /// <summary>
    /// Tasa de recuperación de créditos vencidos
    /// </summary>
    public decimal TasaRecuperacion { get; set; }

    /// <summary>
    /// Monto total de créditos otorgados
    /// </summary>
    public decimal MontoTotalCreditos { get; set; }

    /// <summary>
    /// Monto total de créditos vencidos
    /// </summary>
    public decimal MontoCreditosVencidos { get; set; }

    /// <summary>
    /// Monto total recuperado
    /// </summary>
    public decimal MontoRecuperado { get; set; }

    /// <summary>
    /// Promedio de tiempo de aprobación de créditos (en días)
    /// </summary>
    public decimal PromedioTiempoAprobacion { get; set; }

    /// <summary>
    /// Promedio de monto de créditos otorgados
    /// </summary>
    public decimal PromedioMontoCredito { get; set; }

    /// <summary>
    /// Distribución de tipos de crédito
    /// </summary>
    public Dictionary<string, decimal> DistribucionTiposCredito { get; set; }

    /// <summary>
    /// Distribución de plazos de crédito
    /// </summary>
    public Dictionary<string, decimal> DistribucionPlazos { get; set; }

    /// <summary>
    /// Tendencia de morosidad por mes
    /// </summary>
    public Dictionary<string, decimal> TendenciaMorosidad { get; set; }
} 