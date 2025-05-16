using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Rol;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de roles del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolService _rolService;
    private readonly ILogger<RolesController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="rolService">Servicio de roles</param>
    /// <param name="logger">Logger para el controlador</param>
    public RolesController(
        IRolService rolService,
        ILogger<RolesController> logger)
    {
        _rolService = rolService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los roles del sistema
    /// </summary>
    /// <returns>Lista de roles</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RolDTO>>> GetAll()
    {
        try
        {
            var roles = await _rolService.GetAllAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los roles");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un rol específico por su ID
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <returns>Datos del rol</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<RolDTO>> GetById(int id)
    {
        try
        {
            var rol = await _rolService.GetByIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el rol con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    /// <param name="rolDto">Datos del rol a crear</param>
    /// <returns>Datos del rol creado</returns>
    [HttpPost]
    public async Task<ActionResult<RolDTO>> Create(RolDTO rolDto)
    {
        try
        {
            var rol = await _rolService.CreateAsync(rolDto);
            return CreatedAtAction(nameof(GetById), new { id = rol.Id }, rol);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el rol");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <param name="rolDto">Datos actualizados del rol</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RolDTO rolDto)
    {
        try
        {
            await _rolService.UpdateAsync(id, rolDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el rol con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina un rol
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _rolService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el rol con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los permisos de un rol específico
    /// </summary>
    /// <param name="rolId">ID del rol</param>
    /// <returns>Lista de permisos del rol</returns>
    [HttpGet("{rolId}/permisos")]
    public async Task<ActionResult<IEnumerable<PermisoDTO>>> GetPermisos(int rolId)
    {
        try
        {
            var permisos = await _rolService.GetPermisosAsync(rolId);
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los permisos del rol con ID {RolId}", rolId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Asigna permisos a un rol
    /// </summary>
    /// <param name="rolId">ID del rol</param>
    /// <param name="permisosIds">Lista de IDs de permisos a asignar</param>
    /// <returns>Sin contenido</returns>
    [HttpPost("{rolId}/permisos")]
    public async Task<IActionResult> AsignarPermisos(int rolId, [FromBody] List<int> permisosIds)
    {
        try
        {
            await _rolService.AsignarPermisosAsync(rolId, permisosIds);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al asignar permisos al rol con ID {RolId}", rolId);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 