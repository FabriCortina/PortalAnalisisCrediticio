using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Garantia;

public class CreateGarantiaDTO
{
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoGarantia { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Descripcion { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal ValorGarantia { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Moneda { get; set; }
    
    [Required]
    public DateTime FechaVencimiento { get; set; }
    
    [Required]
    public bool Activa { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [MaxLength(100)]
    public string NumeroDocumento { get; set; }
    
    [MaxLength(200)]
    public string EntidadEmisora { get; set; }
    
    public DateTime? FechaEmision { get; set; }
} 