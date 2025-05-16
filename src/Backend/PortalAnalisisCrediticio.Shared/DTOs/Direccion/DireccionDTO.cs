using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Direccion;

public class DireccionDTO
{
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string TipoDireccion { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Calle { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string Numero { get; set; }
    
    [MaxLength(50)]
    public string Piso { get; set; }
    
    [MaxLength(50)]
    public string Departamento { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Ciudad { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Provincia { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string CodigoPostal { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Pais { get; set; }
    
    [Required]
    public bool Principal { get; set; }
    
    [Required]
    public bool Activa { get; set; }
    
    [MaxLength(500)]
    public string Referencias { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    public DateTime FechaRegistro { get; set; }
    
    public DateTime? FechaActualizacion { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    public decimal? Latitud { get; set; }
    
    public decimal? Longitud { get; set; }
} 