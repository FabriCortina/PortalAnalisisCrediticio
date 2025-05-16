using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Integracion;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de integraciones externas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IntegracionesController : ControllerBase
{
    private readonly IIntegracionService _integracionService;
    private readonly ILogger<IntegracionesController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="integracionService">Servicio de integraciones</param>
    /// <param name="logger">Logger para el controlador</param>
    public IntegracionesController(
        IIntegracionService integracionService,
        ILogger<IntegracionesController> logger)
    {
        _integracionService = integracionService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las integraciones configuradas
    /// </summary>
    /// <returns>Lista de integraciones</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IntegracionDTO>>> GetAll()
    {
        try
        {
            var integraciones = await _integracionService.GetAllAsync();
            return Ok(integraciones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las integraciones");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una integración específica por su ID
    /// </summary>
    /// <param name="id">ID de la integración</param>
    /// <returns>Datos de la integración</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<IntegracionDTO>> GetById(int id)
    {
        try
        {
            var integracion = await _integracionService.GetByIdAsync(id);
            if (integracion == null)
            {
                return NotFound();
            }
            return Ok(integracion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la integración con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea una nueva integración
    /// </summary>
    /// <param name="integracionDto">Datos de la integración a crear</param>
    /// <returns>Datos de la integración creada</returns>
    [HttpPost]
    public async Task<ActionResult<IntegracionDTO>> Create(IntegracionDTO integracionDto)
    {
        try
        {
            var integracion = await _integracionService.CreateAsync(integracionDto);
            return CreatedAtAction(nameof(GetById), new { id = integracion.Id }, integracion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear la integración");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza una integración existente
    /// </summary>
    /// <param name="id">ID de la integración</param>
    /// <param name="integracionDto">Datos actualizados de la integración</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, IntegracionDTO integracionDto)
    {
        try
        {
            await _integracionService.UpdateAsync(id, integracionDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar la integración con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina una integración
    /// </summary>
    /// <param name="id">ID de la integración</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _integracionService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la integración con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Prueba una integración específica
    /// </summary>
    /// <param name="id">ID de la integración</param>
    /// <returns>Resultado de la prueba</returns>
    [HttpPost("{id}/test")]
    public async Task<ActionResult<TestIntegracionDTO>> TestIntegracion(int id)
    {
        try
        {
            var resultado = await _integracionService.TestIntegracionAsync(id);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al probar la integración con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 