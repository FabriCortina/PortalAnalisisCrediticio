using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que representa un crédito activo en el sistema
/// </summary>
public class CreditoActivoDTO
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
    /// Monto del crédito
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// Tasa de interés aplicada
    /// </summary>
    public decimal TasaInteres { get; set; }

    /// <summary>
    /// Fecha de otorgamiento del crédito
    /// </summary>
    public DateTime FechaOtorgamiento { get; set; }

    /// <summary>
    /// Fecha de vencimiento del crédito
    /// </summary>
    public DateTime FechaVencimiento { get; set; }

    /// <summary>
    /// Estado actual del crédito
    /// </summary>
    public string Estado { get; set; }

    /// <summary>
    /// Saldo actual del crédito
    /// </summary>
    public decimal SaldoActual { get; set; }

    /// <summary>
    /// Próxima fecha de pago
    /// </summary>
    public DateTime ProximaFechaPago { get; set; }

    /// <summary>
    /// Monto de la próxima cuota
    /// </summary>
    public decimal MontoProximaCuota { get; set; }

    /// <summary>
    /// Días de atraso (si aplica)
    /// </summary>
    public int? DiasAtraso { get; set; }
} 