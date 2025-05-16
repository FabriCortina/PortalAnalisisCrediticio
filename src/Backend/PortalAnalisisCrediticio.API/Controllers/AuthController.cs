using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Auth;

namespace PortalAnalisisCrediticio.API.Controllers;

/// <summary>
/// Controlador para la gestión de autenticación y autorización
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor del controlador
    /// </summary>
    /// <param name="authService">Servicio de autenticación</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Inicia sesión en el sistema
    /// </summary>
    /// <param name="loginDto">Datos de inicio de sesión</param>
    /// <returns>Datos del usuario y token de acceso</returns>
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioDTO>> Login(LoginDTO loginDto)
    {
        var usuario = await _authService.LoginAsync(loginDto);
        if (usuario == null)
            return Unauthorized("Credenciales inválidas");

        return Ok(usuario);
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    /// <param name="registroDto">Datos de registro</param>
    /// <returns>Datos del usuario registrado</returns>
    [HttpPost("registro")]
    public async Task<ActionResult<UsuarioDTO>> Registro(RegistroDTO registroDto)
    {
        try
        {
            var usuario = await _authService.RegistroAsync(registroDto);
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cambia la contraseña del usuario actual
    /// </summary>
    /// <param name="passwordActual">Contraseña actual</param>
    /// <param name="passwordNuevo">Nueva contraseña</param>
    /// <returns>Sin contenido</returns>
    [Authorize]
    [HttpPost("cambiar-password")]
    public async Task<ActionResult> CambiarPassword(string passwordActual, string passwordNuevo)
    {
        var usuarioId = int.Parse(User.FindFirst("sub")?.Value);
        var resultado = await _authService.CambiarPasswordAsync(usuarioId, passwordActual, passwordNuevo);
        
        if (!resultado)
            return BadRequest("La contraseña actual es incorrecta");

        return Ok();
    }

    /// <summary>
    /// Inicia el proceso de recuperación de contraseña
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <returns>Sin contenido</returns>
    [HttpPost("recuperar-password")]
    public async Task<ActionResult> RecuperarPassword(string email)
    {
        var resultado = await _authService.RecuperarPasswordAsync(email);
        if (!resultado)
            return BadRequest("El email no está registrado");

        return Ok();
    }

    /// <summary>
    /// Obtiene los permisos del usuario actual
    /// </summary>
    /// <returns>Lista de permisos</returns>
    [Authorize]
    [HttpGet("permisos")]
    public async Task<ActionResult<List<PermisoDTO>>> GetPermisos()
    {
        var usuarioId = int.Parse(User.FindFirst("sub")?.Value);
        var permisos = await _authService.GetPermisosAsync(usuarioId);
        return Ok(permisos);
    }

    /// <summary>
    /// Actualiza los permisos de un usuario
    /// </summary>
    /// <param name="usuarioId">ID del usuario</param>
    /// <param name="permisos">Lista de permisos</param>
    /// <returns>Sin contenido</returns>
    [Authorize(Roles = "Administrador")]
    [HttpPut("permisos/{usuarioId}")]
    public async Task<ActionResult> ActualizarPermisos(int usuarioId, List<PermisoDTO> permisos)
    {
        var resultado = await _authService.ActualizarPermisosAsync(usuarioId, permisos);
        if (!resultado)
            return BadRequest("Error al actualizar los permisos");

        return Ok();
    }
} 