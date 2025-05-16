using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Entrevista;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntrevistasController : ControllerBase
{
    private readonly IEntrevistaService _entrevistaService;
    private readonly ILogger<EntrevistasController> _logger;

    public EntrevistasController(
        IEntrevistaService entrevistaService,
        ILogger<EntrevistasController> logger)
    {
        _entrevistaService = entrevistaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EntrevistaDTO>>> GetAll()
    {
        try
        {
            var entrevistas = await _entrevistaService.GetAllAsync();
            return Ok(entrevistas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las entrevistas");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntrevistaDTO>> GetById(int id)
    {
        try
        {
            var entrevista = await _entrevistaService.GetByIdAsync(id);
            if (entrevista == null)
            {
                return NotFound();
            }
            return Ok(entrevista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la entrevista con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<EntrevistaDTO>>> GetByClienteId(int clienteId)
    {
        try
        {
            var entrevistas = await _entrevistaService.GetByClienteIdAsync(clienteId);
            return Ok(entrevistas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las entrevistas del cliente {ClienteId}", clienteId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("solicitud/{solicitudId}")]
    public async Task<ActionResult<IEnumerable<EntrevistaDTO>>> GetBySolicitudId(int solicitudId)
    {
        try
        {
            var entrevistas = await _entrevistaService.GetBySolicitudIdAsync(solicitudId);
            return Ok(entrevistas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener las entrevistas de la solicitud {SolicitudId}", solicitudId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<EntrevistaDTO>> Create(CreateEntrevistaDTO entrevistaDto)
    {
        try
        {
            var entrevista = await _entrevistaService.CreateAsync(entrevistaDto);
            return CreatedAtAction(nameof(GetById), new { id = entrevista.Id }, entrevista);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear la entrevista");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la entrevista");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EntrevistaDTO>> Update(int id, UpdateEntrevistaDTO entrevistaDto)
    {
        try
        {
            var entrevista = await _entrevistaService.UpdateAsync(id, entrevistaDto);
            if (entrevista == null)
            {
                return NotFound();
            }
            return Ok(entrevista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la entrevista con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _entrevistaService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la entrevista con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 