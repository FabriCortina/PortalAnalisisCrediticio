using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Parametro;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de parámetros del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ParametrosController : ControllerBase
{
    private readonly IParametroService _parametroService;
    private readonly ILogger<ParametrosController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="parametroService">Servicio de parámetros</param>
    /// <param name="logger">Logger para el controlador</param>
    public ParametrosController(
        IParametroService parametroService,
        ILogger<ParametrosController> logger)
    {
        _parametroService = parametroService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los parámetros del sistema
    /// </summary>
    /// <returns>Lista de parámetros</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParametroDTO>>> GetAll()
    {
        try
        {
            var parametros = await _parametroService.GetAllAsync();
            return Ok(parametros);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los parámetros");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un parámetro específico por su clave
    /// </summary>
    /// <param name="clave">Clave del parámetro</param>
    /// <returns>Datos del parámetro</returns>
    [HttpGet("{clave}")]
    public async Task<ActionResult<ParametroDTO>> GetByClave(string clave)
    {
        try
        {
            var parametro = await _parametroService.GetByClaveAsync(clave);
            if (parametro == null)
            {
                return NotFound();
            }
            return Ok(parametro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el parámetro con clave {Clave}", clave);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un parámetro existente
    /// </summary>
    /// <param name="clave">Clave del parámetro</param>
    /// <param name="parametroDto">Datos actualizados del parámetro</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{clave}")]
    public async Task<IActionResult> Update(string clave, ParametroDTO parametroDto)
    {
        try
        {
            await _parametroService.UpdateAsync(clave, parametroDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el parámetro con clave {Clave}", clave);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene el valor de un parámetro específico
    /// </summary>
    /// <param name="clave">Clave del parámetro</param>
    /// <returns>Valor del parámetro</returns>
    [HttpGet("{clave}/valor")]
    public async Task<ActionResult<string>> GetValor(string clave)
    {
        try
        {
            var valor = await _parametroService.GetValorAsync(clave);
            if (valor == null)
            {
                return NotFound();
            }
            return Ok(valor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el valor del parámetro con clave {Clave}", clave);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 