using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Rol;

public class CreateRolDTO
{
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Descripcion { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    public List<string> Permisos { get; set; }
} 