using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

namespace PortalAnalisisCrediticio.Core.Interfaces;

/// <summary>
/// Servicio para la realización de análisis de riesgo crediticio
/// </summary>
public interface IAnalisisRiesgoService
{
    /// <summary>
    /// Realiza un análisis de riesgo completo para un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente a analizar</param>
    /// <returns>Informe de riesgo con recomendaciones</returns>
    Task<InformeRiesgoDTO> RealizarAnalisisRiesgoAsync(int clienteId);

    /// <summary>
    /// Obtiene el último informe de riesgo de un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Último informe de riesgo</returns>
    Task<InformeRiesgoDTO> ObtenerUltimoInformeAsync(int clienteId);

    /// <summary>
    /// Obtiene el historial de informes de riesgo de un cliente
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Lista de informes de riesgo</returns>
    Task<IEnumerable<InformeRiesgoDTO>> ObtenerHistorialInformesAsync(int clienteId);

    /// <summary>
    /// Genera un informe PDF del análisis de riesgo
    /// </summary>
    /// <param name="informeId">ID del informe</param>
    /// <returns>Archivo PDF del informe</returns>
    Task<byte[]> GenerarInformePDFAsync(int informeId);
} 