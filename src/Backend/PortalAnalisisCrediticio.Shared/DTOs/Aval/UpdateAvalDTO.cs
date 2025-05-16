using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Aval;

public class UpdateAvalDTO
{
    [Required]
    [MaxLength(100)]
    public string NombreCompleto { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoDocumento { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string NumeroDocumento { get; set; }
    
    [Required]
    public DateTime FechaNacimiento { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Nacionalidad { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Ocupacion { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Empresa { get; set; }
    
    [Required]
    public decimal IngresosMensuales { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Moneda { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
} 