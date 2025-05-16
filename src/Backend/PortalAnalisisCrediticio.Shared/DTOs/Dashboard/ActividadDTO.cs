using System;
using System.Collections.Generic;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que representa una actividad reciente en el sistema
/// </summary>
public class ActividadDTO
{
    /// <summary>
    /// ID único de la actividad
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tipo de actividad realizada
    /// </summary>
    public string Tipo { get; set; }

    /// <summary>
    /// Descripción detallada de la actividad
    /// </summary>
    public string Descripcion { get; set; }

    /// <summary>
    /// Fecha y hora de la actividad
    /// </summary>
    public DateTime FechaHora { get; set; }

    /// <summary>
    /// Usuario que realizó la actividad
    /// </summary>
    public string Usuario { get; set; }

    /// <summary>
    /// ID del cliente relacionado (si aplica)
    /// </summary>
    public int? ClienteId { get; set; }

    /// <summary>
    /// Nombre del cliente relacionado (si aplica)
    /// </summary>
    public string NombreCliente { get; set; }

    /// <summary>
    /// Estado o resultado de la actividad
    /// </summary>
    public string Estado { get; set; }

    /// <summary>
    /// Datos adicionales relacionados con la actividad
    /// </summary>
    public Dictionary<string, string> DatosAdicionales { get; set; }
} 