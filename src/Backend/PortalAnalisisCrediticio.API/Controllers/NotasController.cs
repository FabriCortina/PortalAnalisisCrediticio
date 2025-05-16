using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Nota;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de notas y comentarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotasController : ControllerBase
{
    private readonly INotaService _notaService;
    private readonly ILogger<NotasController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="notaService">Servicio de notas</param>
    /// <param name="logger">Logger para el controlador</param>
    public NotasController(
        INotaService notaService,
        ILogger<NotasController> logger)
    {
        _notaService = notaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las notas
    /// </summary>
    /// <returns>Lista de notas</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotaDTO>>> GetAll()
    {
        try
        {
            var notas = await _notaService.GetAllAsync();
            return Ok(notas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las notas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una nota específica por su ID
    /// </summary>
    /// <param name="id">ID de la nota</param>
    /// <returns>Datos de la nota</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<NotaDTO>> GetById(int id)
    {
        try
        {
            var nota = await _notaService.GetByIdAsync(id);
            if (nota == null)
            {
                return NotFound();
            }
            return Ok(nota);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la nota con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<NotaDTO>>> GetByClienteId(int clienteId)
    {
        try
        {
            var notas = await _notaService.GetByClienteIdAsync(clienteId);
            return Ok(notas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las notas del cliente {ClienteId}", clienteId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("solicitud/{solicitudId}")]
    public async Task<ActionResult<IEnumerable<NotaDTO>>> GetBySolicitudId(int solicitudId)
    {
        try
        {
            var notas = await _notaService.GetBySolicitudIdAsync(solicitudId);
            return Ok(notas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las notas de la solicitud {SolicitudId}", solicitudId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<NotaDTO>> Create(CreateNotaDTO notaDto)
    {
        try
        {
            var nota = await _notaService.CreateAsync(notaDto);
            return CreatedAtAction(nameof(GetById), new { id = nota.Id }, nota);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear la nota");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la nota");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NotaDTO>> Update(int id, UpdateNotaDTO notaDto)
    {
        try
        {
            var nota = await _notaService.UpdateAsync(id, notaDto);
            if (nota == null)
            {
                return NotFound();
            }
            return Ok(nota);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la nota con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _notaService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la nota con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 