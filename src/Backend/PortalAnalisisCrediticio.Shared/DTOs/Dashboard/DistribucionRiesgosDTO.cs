namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard;

/// <summary>
/// DTO que contiene la distribución de clientes por nivel de riesgo
/// </summary>
public class DistribucionRiesgosDTO
{
    /// <summary>
    /// Cantidad de clientes con riesgo bajo
    /// </summary>
    public int RiesgoBajo { get; set; }

    /// <summary>
    /// Cantidad de clientes con riesgo medio
    /// </summary>
    public int RiesgoMedio { get; set; }

    /// <summary>
    /// Cantidad de clientes con riesgo alto
    /// </summary>
    public int RiesgoAlto { get; set; }

    /// <summary>
    /// Cantidad de clientes con riesgo crítico
    /// </summary>
    public int RiesgoCritico { get; set; }

    /// <summary>
    /// Porcentaje de clientes con riesgo bajo
    /// </summary>
    public decimal PorcentajeRiesgoBajo { get; set; }

    /// <summary>
    /// Porcentaje de clientes con riesgo medio
    /// </summary>
    public decimal PorcentajeRiesgoMedio { get; set; }

    /// <summary>
    /// Porcentaje de clientes con riesgo alto
    /// </summary>
    public decimal PorcentajeRiesgoAlto { get; set; }

    /// <summary>
    /// Porcentaje de clientes con riesgo crítico
    /// </summary>
    public decimal PorcentajeRiesgoCritico { get; set; }
} 