using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Permiso;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de permisos del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PermisosController : ControllerBase
{
    private readonly IPermisoService _permisoService;
    private readonly ILogger<PermisosController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="permisoService">Servicio de permisos</param>
    /// <param name="logger">Logger para el controlador</param>
    public PermisosController(
        IPermisoService permisoService,
        ILogger<PermisosController> logger)
    {
        _permisoService = permisoService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los permisos del sistema
    /// </summary>
    /// <returns>Lista de permisos</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermisoDTO>>> GetAll()
    {
        try
        {
            var permisos = await _permisoService.GetAllAsync();
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los permisos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un permiso específico por su ID
    /// </summary>
    /// <param name="id">ID del permiso</param>
    /// <returns>Datos del permiso</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PermisoDTO>> GetById(int id)
    {
        try
        {
            var permiso = await _permisoService.GetByIdAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            return Ok(permiso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el permiso con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo permiso
    /// </summary>
    /// <param name="permisoDto">Datos del permiso a crear</param>
    /// <returns>Datos del permiso creado</returns>
    [HttpPost]
    public async Task<ActionResult<PermisoDTO>> Create(PermisoDTO permisoDto)
    {
        try
        {
            var permiso = await _permisoService.CreateAsync(permisoDto);
            return CreatedAtAction(nameof(GetById), new { id = permiso.Id }, permiso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el permiso");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un permiso existente
    /// </summary>
    /// <param name="id">ID del permiso</param>
    /// <param name="permisoDto">Datos actualizados del permiso</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PermisoDTO permisoDto)
    {
        try
        {
            await _permisoService.UpdateAsync(id, permisoDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el permiso con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina un permiso
    /// </summary>
    /// <param name="id">ID del permiso</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _permisoService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el permiso con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los permisos de un usuario específico
    /// </summary>
    /// <param name="usuarioId">ID del usuario</param>
    /// <returns>Lista de permisos del usuario</returns>
    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<PermisoDTO>>> GetByUsuario(int usuarioId)
    {
        try
        {
            var permisos = await _permisoService.GetByUsuarioAsync(usuarioId);
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los permisos del usuario con ID {UsuarioId}", usuarioId);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 