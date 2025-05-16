using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Historial;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión del historial de cambios
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HistorialController : ControllerBase
{
    private readonly IHistorialService _historialService;
    private readonly ILogger<HistorialController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="historialService">Servicio de historial</param>
    /// <param name="logger">Logger para el controlador</param>
    public HistorialController(
        IHistorialService historialService,
        ILogger<HistorialController> logger)
    {
        _historialService = historialService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene el historial de cambios de una entidad
    /// </summary>
    /// <param name="entidadId">ID de la entidad</param>
    /// <param name="tipoEntidad">Tipo de entidad</param>
    /// <returns>Lista de cambios en el historial</returns>
    [HttpGet("{entidadId}/{tipoEntidad}")]
    public async Task<ActionResult<IEnumerable<HistorialDTO>>> GetHistorial(int entidadId, string tipoEntidad)
    {
        try
        {
            var historial = await _historialService.GetHistorialAsync(entidadId, tipoEntidad);
            return Ok(historial);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el historial para la entidad {EntidadId} de tipo {TipoEntidad}", entidadId, tipoEntidad);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Registra un nuevo cambio en el historial
    /// </summary>
    /// <param name="historialDto">Datos del cambio a registrar</param>
    /// <returns>Datos del cambio registrado</returns>
    [HttpPost]
    public async Task<ActionResult<HistorialDTO>> RegistrarCambio(HistorialDTO historialDto)
    {
        try
        {
            var historial = await _historialService.RegistrarCambioAsync(historialDto);
            return CreatedAtAction(nameof(GetHistorial), new { entidadId = historial.EntidadId, tipoEntidad = historial.TipoEntidad }, historial);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar cambio en el historial");
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 