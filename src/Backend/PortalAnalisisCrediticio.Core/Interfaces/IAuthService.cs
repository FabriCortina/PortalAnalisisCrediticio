using PortalAnalisisCrediticio.Shared.DTOs.Auth;

namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IAuthService
{
    Task<UsuarioDTO> LoginAsync(LoginDTO loginDto);
    Task<UsuarioDTO> RegistroAsync(RegistroDTO registroDto);
    Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNuevo);
    Task<bool> RecuperarPasswordAsync(string email);
    Task<bool> VerificarPermisoAsync(int usuarioId, string recurso, string accion);
    Task<List<PermisoDTO>> GetPermisosAsync(int usuarioId);
    Task<bool> ActualizarPermisosAsync(int usuarioId, List<PermisoDTO> permisos);
} 