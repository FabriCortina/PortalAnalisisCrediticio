using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Garantia;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de garantías
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GarantiasController : ControllerBase
{
    private readonly IGarantiaService _garantiaService;
    private readonly ILogger<GarantiasController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="garantiaService">Servicio de garantías</param>
    /// <param name="logger">Logger para el controlador</param>
    public GarantiasController(
        IGarantiaService garantiaService,
        ILogger<GarantiasController> logger)
    {
        _garantiaService = garantiaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las garantías
    /// </summary>
    /// <returns>Lista de garantías</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GarantiaDTO>>> GetAll()
    {
        try
        {
            var garantias = await _garantiaService.GetAllAsync();
            return Ok(garantias);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las garantías");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una garantía específica por su ID
    /// </summary>
    /// <param name="id">ID de la garantía</param>
    /// <returns>Datos de la garantía</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GarantiaDTO>> GetById(int id)
    {
        try
        {
            var garantia = await _garantiaService.GetByIdAsync(id);
            if (garantia == null)
            {
                return NotFound();
            }
            return Ok(garantia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la garantía con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<GarantiaDTO>>> GetByClienteId(int clienteId)
    {
        try
        {
            var garantias = await _garantiaService.GetByClienteIdAsync(clienteId);
            return Ok(garantias);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las garantías del cliente {ClienteId}", clienteId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("solicitud/{solicitudId}")]
    public async Task<ActionResult<IEnumerable<GarantiaDTO>>> GetBySolicitudId(int solicitudId)
    {
        try
        {
            var garantias = await _garantiaService.GetBySolicitudIdAsync(solicitudId);
            return Ok(garantias);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las garantías de la solicitud {SolicitudId}", solicitudId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<GarantiaDTO>> Create(CreateGarantiaDTO garantiaDto)
    {
        try
        {
            var garantia = await _garantiaService.CreateAsync(garantiaDto);
            return CreatedAtAction(nameof(GetById), new { id = garantia.Id }, garantia);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear la garantía");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la garantía");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GarantiaDTO>> Update(int id, UpdateGarantiaDTO garantiaDto)
    {
        try
        {
            var garantia = await _garantiaService.UpdateAsync(id, garantiaDto);
            if (garantia == null)
            {
                return NotFound();
            }
            return Ok(garantia);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la garantía con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _garantiaService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la garantía con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 