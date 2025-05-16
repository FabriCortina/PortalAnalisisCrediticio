using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.CondicionPago;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CondicionesPagoController : ControllerBase
{
    private readonly ICondicionPagoService _condicionPagoService;
    private readonly ILogger<CondicionesPagoController> _logger;

    public CondicionesPagoController(
        ICondicionPagoService condicionPagoService,
        ILogger<CondicionesPagoController> logger)
    {
        _condicionPagoService = condicionPagoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CondicionPagoDTO>>> GetAll()
    {
        try
        {
            var condiciones = await _condicionPagoService.GetAllAsync();
            return Ok(condiciones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las condiciones de pago");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CondicionPagoDTO>> GetById(int id)
    {
        try
        {
            var condicion = await _condicionPagoService.GetByIdAsync(id);
            if (condicion == null)
            {
                return NotFound();
            }
            return Ok(condicion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la condición de pago con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("solicitud/{solicitudId}")]
    public async Task<ActionResult<CondicionPagoDTO>> GetBySolicitudId(int solicitudId)
    {
        try
        {
            var condicion = await _condicionPagoService.GetBySolicitudIdAsync(solicitudId);
            if (condicion == null)
            {
                return NotFound();
            }
            return Ok(condicion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la condición de pago para la solicitud {SolicitudId}", solicitudId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CondicionPagoDTO>> Create(CreateCondicionPagoDTO condicionPagoDto)
    {
        try
        {
            var condicion = await _condicionPagoService.CreateAsync(condicionPagoDto);
            return CreatedAtAction(nameof(GetById), new { id = condicion.Id }, condicion);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear la condición de pago");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la condición de pago");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CondicionPagoDTO>> Update(int id, UpdateCondicionPagoDTO condicionPagoDto)
    {
        try
        {
            var condicion = await _condicionPagoService.UpdateAsync(id, condicionPagoDto);
            if (condicion == null)
            {
                return NotFound();
            }
            return Ok(condicion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la condición de pago con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _condicionPagoService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la condición de pago con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 