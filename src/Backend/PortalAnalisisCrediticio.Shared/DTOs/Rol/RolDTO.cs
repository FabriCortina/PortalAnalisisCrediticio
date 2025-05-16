using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Rol;

public class RolDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Descripcion { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [Required]
    public DateTime FechaRegistro { get; set; }
    
    public DateTime? FechaActualizacion { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
    
    [Required]
    public int NivelAcceso { get; set; }
    
    [Required]
    public bool EsAdministrador { get; set; }
    
    [Required]
    public bool PuedeCrearUsuarios { get; set; }
    
    [Required]
    public bool PuedeModificarUsuarios { get; set; }
    
    [Required]
    public bool PuedeEliminarUsuarios { get; set; }
    
    [Required]
    public bool PuedeVerReportes { get; set; }
    
    [Required]
    public bool PuedeGenerarReportes { get; set; }
    
    [Required]
    public bool PuedeConfigurarSistema { get; set; }
    
    [Required]
    public bool PuedeGestionarRoles { get; set; }
    
    [Required]
    public bool PuedeGestionarPermisos { get; set; }
} 