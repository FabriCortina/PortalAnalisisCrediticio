using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required]
    [MaxLength(50)]
    public string NombreUsuario { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string NombreCompleto { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Rol { get; set; }
    
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Password { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [MaxLength(50)]
    public string Departamento { get; set; }
    
    [MaxLength(50)]
    public string Cargo { get; set; }
    
    [MaxLength(20)]
    public string Telefono { get; set; }
    
    [MaxLength(20)]
    public string TelefonoAlternativo { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
} 