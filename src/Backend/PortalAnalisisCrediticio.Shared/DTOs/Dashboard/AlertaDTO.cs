namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que representa una alerta del sistema
/// </summary>
public class AlertaDTO
{
    /// <summary>
    /// ID único de la alerta
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tipo de alerta (Riesgo, Vencimiento, etc.)
    /// </summary>
    public string Tipo { get; set; }

    /// <summary>
    /// Nivel de severidad de la alerta (Bajo, Medio, Alto, Crítico)
    /// </summary>
    public string Severidad { get; set; }

    /// <summary>
    /// Mensaje descriptivo de la alerta
    /// </summary>
    public string Mensaje { get; set; }

    /// <summary>
    /// Fecha y hora de generación de la alerta
    /// </summary>
    public DateTime FechaGeneracion { get; set; }

    /// <summary>
    /// ID del cliente relacionado con la alerta
    /// </summary>
    public int? ClienteId { get; set; }

    /// <summary>
    /// Nombre del cliente relacionado con la alerta
    /// </summary>
    public string NombreCliente { get; set; }

    /// <summary>
    /// Indica si la alerta ha sido leída
    /// </summary>
    public bool Leida { get; set; }

    /// <summary>
    /// Fecha y hora en que la alerta fue leída
    /// </summary>
    public DateTime? FechaLectura { get; set; }
} 