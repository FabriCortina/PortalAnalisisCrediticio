using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Contacto;

public class ContactoDTO
{
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string NombreCompleto { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoContacto { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Cargo { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Telefono { get; set; }
    
    [MaxLength(50)]
    public string TelefonoAlternativo { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }
    
    [MaxLength(100)]
    public string EmailAlternativo { get; set; }
    
    [Required]
    public bool Principal { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    public DateTime FechaRegistro { get; set; }
    
    public DateTime? FechaActualizacion { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [MaxLength(100)]
    public string Departamento { get; set; }
    
    [MaxLength(100)]
    public string Empresa { get; set; }
    
    public DateTime? FechaNacimiento { get; set; }
    
    [MaxLength(50)]
    public string TipoDocumento { get; set; }
    
    [MaxLength(20)]
    public string NumeroDocumento { get; set; }
} 