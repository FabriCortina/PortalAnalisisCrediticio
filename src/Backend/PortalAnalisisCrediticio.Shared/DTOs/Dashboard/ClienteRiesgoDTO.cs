using System;
using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que representa un cliente en situación de riesgo
/// </summary>
public class ClienteRiesgoDTO
{
    /// <summary>
    /// ID único del cliente
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre completo del cliente
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Número de documento de identidad
    /// </summary>
    public string Documento { get; set; }

    /// <summary>
    /// Nivel de riesgo asignado (Bajo, Medio, Alto, Crítico)
    /// </summary>
    public string NivelRiesgo { get; set; }

    /// <summary>
    /// Puntaje de riesgo calculado
    /// </summary>
    public decimal PuntajeRiesgo { get; set; }

    /// <summary>
    /// Fecha del último análisis de riesgo
    /// </summary>
    public DateTime FechaUltimoAnalisis { get; set; }

    /// <summary>
    /// Monto total de créditos activos
    /// </summary>
    public decimal MontoTotalCreditos { get; set; }

    /// <summary>
    /// Cantidad de créditos activos
    /// </summary>
    public int CantidadCreditos { get; set; }

    /// <summary>
    /// Días de atraso en pagos (si aplica)
    /// </summary>
    public int? DiasAtraso { get; set; }

    /// <summary>
    /// Razones principales del riesgo
    /// </summary>
    public List<string> RazonesRiesgo { get; set; }

    /// <summary>
    /// Recomendaciones para mitigar el riesgo
    /// </summary>
    public List<string> Recomendaciones { get; set; }
} 