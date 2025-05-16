using System;
using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que representa un crédito vencido en el sistema
/// </summary>
public class CreditoVencidoDTO
{
    /// <summary>
    /// ID único del crédito
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID del cliente al que pertenece el crédito
    /// </summary>
    public int ClienteId { get; set; }

    /// <summary>
    /// Nombre del cliente
    /// </summary>
    public string NombreCliente { get; set; }

    /// <summary>
    /// Monto original del crédito
    /// </summary>
    public decimal MontoOriginal { get; set; }

    /// <summary>
    /// Saldo actual del crédito
    /// </summary>
    public decimal SaldoActual { get; set; }

    /// <summary>
    /// Fecha de vencimiento del crédito
    /// </summary>
    public DateTime FechaVencimiento { get; set; }

    /// <summary>
    /// Días de atraso desde el vencimiento
    /// </summary>
    public int DiasAtraso { get; set; }

    /// <summary>
    /// Monto de intereses moratorios acumulados
    /// </summary>
    public decimal InteresesMoratorios { get; set; }

    /// <summary>
    /// Estado actual del crédito
    /// </summary>
    public string Estado { get; set; }

    /// <summary>
    /// Última fecha de pago registrada
    /// </summary>
    public DateTime? UltimaFechaPago { get; set; }

    /// <summary>
    /// Monto del último pago realizado
    /// </summary>
    public decimal? MontoUltimoPago { get; set; }

    /// <summary>
    /// Acciones tomadas para la recuperación
    /// </summary>
    public List<string> AccionesRecuperacion { get; set; }
} 