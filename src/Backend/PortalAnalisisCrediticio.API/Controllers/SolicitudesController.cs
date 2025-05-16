using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Solicitud;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de solicitudes de crédito
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SolicitudesController : ControllerBase
{
    private readonly ISolicitudService _solicitudService;
    private readonly ILogger<SolicitudesController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="solicitudService">Servicio de solicitudes</param>
    /// <param name="logger">Logger para el controlador</param>
    public SolicitudesController(
        ISolicitudService solicitudService,
        ILogger<SolicitudesController> logger)
    {
        _solicitudService = solicitudService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las solicitudes
    /// </summary>
    /// <returns>Lista de solicitudes</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitudDTO>>> GetAll()
    {
        try
        {
            var solicitudes = await _solicitudService.GetAllAsync();
            return Ok(solicitudes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las solicitudes");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una solicitud específica por su ID
    /// </summary>
    /// <param name="id">ID de la solicitud</param>
    /// <returns>Datos de la solicitud</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SolicitudDTO>> GetById(int id)
    {
        try
        {
            var solicitud = await _solicitudService.GetByIdAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            return Ok(solicitud);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la solicitud con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva solicitud
    /// </summary>
    /// <param name="solicitudDto">Datos de la solicitud a crear</param>
    /// <returns>Datos de la solicitud creada</returns>
    [HttpPost]
    public async Task<ActionResult<SolicitudDTO>> Create(SolicitudDTO solicitudDto)
    {
        try
        {
            var solicitud = await _solicitudService.CreateAsync(solicitudDto);
            return CreatedAtAction(nameof(GetById), new { id = solicitud.Id }, solicitud);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la solicitud");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza una solicitud existente
    /// </summary>
    /// <param name="id">ID de la solicitud</param>
    /// <param name="solicitudDto">Datos actualizados de la solicitud</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SolicitudDTO solicitudDto)
    {
        try
        {
            await _solicitudService.UpdateAsync(id, solicitudDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la solicitud con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina una solicitud
    /// </summary>
    /// <param name="id">ID de la solicitud</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _solicitudService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la solicitud con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene las solicitudes por estado
    /// </summary>
    /// <param name="estado">Estado de las solicitudes</param>
    /// <returns>Lista de solicitudes en el estado especificado</returns>
    [HttpGet("estado/{estado}")]
    public async Task<ActionResult<IEnumerable<SolicitudDTO>>> GetByEstado(string estado)
    {
        try
        {
            var solicitudes = await _solicitudService.GetByEstadoAsync(estado);
            return Ok(solicitudes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las solicitudes con estado {Estado}", estado);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene las solicitudes de un cliente específico
    /// </summary>
    /// <param name="clienteId">ID del cliente</param>
    /// <returns>Lista de solicitudes del cliente</returns>
    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<SolicitudDTO>>> GetByCliente(int clienteId)
    {
        try
        {
            var solicitudes = await _solicitudService.GetByClienteAsync(clienteId);
            return Ok(solicitudes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las solicitudes del cliente con ID {ClienteId}", clienteId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Cambia el estado de una solicitud
    /// </summary>
    /// <param name="id">ID de la solicitud</param>
    /// <param name="nuevoEstado">Nuevo estado de la solicitud</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{id}/estado")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
    {
        try
        {
            await _solicitudService.CambiarEstadoAsync(id, nuevoEstado);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar el estado de la solicitud con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 