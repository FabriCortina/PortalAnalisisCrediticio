using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Auth;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string Rol { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }
    public List<PermisoDTO> Permisos { get; set; }
}

public class PermisoDTO
{
    public string Recurso { get; set; }
    public string Accion { get; set; }
    public bool Permitido { get; set; }
} 