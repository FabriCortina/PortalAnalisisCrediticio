using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Core.Interfaces;

/// <summary>
/// Servicio para el registro y consulta de logs del sistema
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Registra un nuevo log en el sistema
    /// </summary>
    /// <param name="log">Datos del log a registrar</param>
    Task RegistrarLogAsync(LogDTO log);

    /// <summary>
    /// Obtiene los logs del sistema con paginación y filtros
    /// </summary>
    /// <param name="page">Número de página</param>
    /// <param name="size">Tamaño de página</param>
    /// <param name="filtros">Filtros opcionales</param>
    /// <returns>Lista paginada de logs</returns>
    Task<IEnumerable<LogDTO>> GetLogsAsync(int page, int size, Dictionary<string, string> filtros = null);

    /// <summary>
    /// Obtiene un log específico por su ID
    /// </summary>
    /// <param name="id">ID del log</param>
    /// <returns>Datos del log</returns>
    Task<LogDTO> GetLogByIdAsync(int id);

    /// <summary>
    /// Limpia los logs antiguos
    /// </summary>
    /// <param name="dias">Número de días a mantener</param>
    Task LimpiarLogsAntiguosAsync(int dias);

    /// <summary>
    /// Exporta los logs a un archivo
    /// </summary>
    /// <param name="formato">Formato de exportación (Excel/CSV)</param>
    /// <param name="filtros">Filtros opcionales</param>
    /// <returns>Archivo con los logs exportados</returns>
    Task<byte[]> ExportarLogsAsync(string formato, Dictionary<string, string> filtros = null);
} 