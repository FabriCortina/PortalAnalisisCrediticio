using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Auth;

namespace PortalAnalisisCrediticio.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UsuarioDTO>> Login(LoginDTO loginDto)
    {
        var usuario = await _authService.LoginAsync(loginDto);
        if (usuario == null)
            return Unauthorized("Credenciales inválidas");

        return Ok(usuario);
    }

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

    [HttpPost("recuperar-password")]
    public async Task<ActionResult> RecuperarPassword(string email)
    {
        var resultado = await _authService.RecuperarPasswordAsync(email);
        if (!resultado)
            return BadRequest("El email no está registrado");

        return Ok();
    }

    [Authorize]
    [HttpGet("permisos")]
    public async Task<ActionResult<List<PermisoDTO>>> GetPermisos()
    {
        var usuarioId = int.Parse(User.FindFirst("sub")?.Value);
        var permisos = await _authService.GetPermisosAsync(usuarioId);
        return Ok(permisos);
    }

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