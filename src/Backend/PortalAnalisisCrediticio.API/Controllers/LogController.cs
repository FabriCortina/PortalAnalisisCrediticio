using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de logs del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class LogController : ControllerBase
{
    private readonly ILogService _logService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="logService">Servicio de logs</param>
    public LogController(ILogService logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Obtiene los logs del sistema con paginación y filtros
    /// </summary>
    /// <param name="page">Número de página</param>
    /// <param name="size">Tamaño de página</param>
    /// <param name="filtros">Filtros opcionales</param>
    /// <returns>Lista paginada de logs</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogDTO>>> GetLogs(
        [FromQuery] int page = 1,
        [FromQuery] int size = 50,
        [FromQuery] Dictionary<string, string> filtros = null)
    {
        var logs = await _logService.GetLogsAsync(page, size, filtros);
        return Ok(logs);
    }

    /// <summary>
    /// Obtiene un log específico por su ID
    /// </summary>
    /// <param name="id">ID del log</param>
    /// <returns>Datos del log</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<LogDTO>> GetLogById(int id)
    {
        var log = await _logService.GetLogByIdAsync(id);
        if (log == null)
            return NotFound();

        return Ok(log);
    }

    /// <summary>
    /// Limpia los logs antiguos
    /// </summary>
    /// <param name="dias">Número de días a mantener</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("limpiar")]
    public async Task<ActionResult> LimpiarLogsAntiguos([FromQuery] int dias = 30)
    {
        await _logService.LimpiarLogsAntiguosAsync(dias);
        return NoContent();
    }

    /// <summary>
    /// Exporta los logs a un archivo
    /// </summary>
    /// <param name="formato">Formato de exportación (Excel/CSV)</param>
    /// <param name="filtros">Filtros opcionales</param>
    /// <returns>Archivo con los logs exportados</returns>
    [HttpGet("exportar")]
    public async Task<ActionResult> ExportarLogs(
        [FromQuery] string formato,
        [FromQuery] Dictionary<string, string> filtros = null)
    {
        var archivo = await _logService.ExportarLogsAsync(formato, filtros);
        return File(archivo, GetContentType(formato), $"logs-export.{formato.ToLower()}");
    }

    private string GetContentType(string formato)
    {
        return formato.ToLower() switch
        {
            "excel" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "csv" => "text/csv",
            _ => "application/octet-stream"
        };
    }
} 