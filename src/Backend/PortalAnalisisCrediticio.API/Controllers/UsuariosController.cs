using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Usuario;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de usuarios del sistema
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="usuarioService">Servicio de usuarios</param>
    /// <param name="logger">Logger para el controlador</param>
    public UsuariosController(
        IUsuarioService usuarioService,
        ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los usuarios del sistema
    /// </summary>
    /// <returns>Lista de usuarios</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetAll()
    {
        try
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los usuarios");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un usuario específico por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Datos del usuario</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDTO>> GetById(int id)
    {
        try
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el usuario con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="usuarioDto">Datos del usuario a crear</param>
    /// <returns>Datos del usuario creado</returns>
    [HttpPost]
    public async Task<ActionResult<UsuarioDTO>> Create(UsuarioDTO usuarioDto)
    {
        try
        {
            var usuario = await _usuarioService.CreateAsync(usuarioDto);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el usuario");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="usuarioDto">Datos actualizados del usuario</param>
    /// <returns>Sin contenido</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UsuarioDTO usuarioDto)
    {
        try
        {
            await _usuarioService.UpdateAsync(id, usuarioDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el usuario con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina un usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Sin contenido</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el usuario con ID {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los roles de un usuario específico
    /// </summary>
    /// <param name="usuarioId">ID del usuario</param>
    /// <returns>Lista de roles del usuario</returns>
    [HttpGet("{usuarioId}/roles")]
    public async Task<ActionResult<IEnumerable<RolDTO>>> GetRoles(int usuarioId)
    {
        try
        {
            var roles = await _usuarioService.GetRolesAsync(usuarioId);
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los roles del usuario con ID {UsuarioId}", usuarioId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Asigna roles a un usuario
    /// </summary>
    /// <param name="usuarioId">ID del usuario</param>
    /// <param name="rolesIds">Lista de IDs de roles a asignar</param>
    /// <returns>Sin contenido</returns>
    [HttpPost("{usuarioId}/roles")]
    public async Task<IActionResult> AsignarRoles(int usuarioId, [FromBody] List<int> rolesIds)
    {
        try
        {
            await _usuarioService.AsignarRolesAsync(usuarioId, rolesIds);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al asignar roles al usuario con ID {UsuarioId}", usuarioId);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene los permisos de un usuario específico
    /// </summary>
    /// <param name="usuarioId">ID del usuario</param>
    /// <returns>Lista de permisos del usuario</returns>
    [HttpGet("{usuarioId}/permisos")]
    public async Task<ActionResult<IEnumerable<PermisoDTO>>> GetPermisos(int usuarioId)
    {
        try
        {
            var permisos = await _usuarioService.GetPermisosAsync(usuarioId);
            return Ok(permisos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los permisos del usuario con ID {UsuarioId}", usuarioId);
            return StatusCode(500, "Error interno del servidor");
        }
    }
} 