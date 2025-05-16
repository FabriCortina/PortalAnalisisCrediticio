namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que contiene la distribución de solicitudes por mes
/// </summary>
public class SolicitudesPorMesDTO
{
    /// <summary>
    /// Año de las solicitudes
    /// </summary>
    public int Año { get; set; }

    /// <summary>
    /// Cantidad de solicitudes por mes
    /// </summary>
    public Dictionary<string, int> SolicitudesPorMes { get; set; }

    /// <summary>
    /// Total de solicitudes en el año
    /// </summary>
    public int TotalSolicitudes { get; set; }

    /// <summary>
    /// Promedio de solicitudes por mes
    /// </summary>
    public decimal PromedioMensual { get; set; }

    /// <summary>
    /// Mes con mayor cantidad de solicitudes
    /// </summary>
    public string MesMasSolicitudes { get; set; }

    /// <summary>
    /// Mes con menor cantidad de solicitudes
    /// </summary>
    public string MesMenosSolicitudes { get; set; }
} 