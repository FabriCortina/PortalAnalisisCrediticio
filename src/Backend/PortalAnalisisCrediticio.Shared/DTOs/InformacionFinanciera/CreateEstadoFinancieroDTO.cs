using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

public class CreateEstadoFinancieroDTO
{
    [Required]
    public int InformacionFinancieraId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Ventas { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal CostoVentas { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal GastosOperativos { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal ActivosCorrientes { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal ActivosNoCorrientes { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal PasivosCorrientes { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal PasivosNoCorrientes { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Capital { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 