using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

public class UpdateDeudaDTO
{
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Monto { get; set; }
    
    [Required]
    [Range(0, 100)]
    public decimal TasaInteres { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Plazo { get; set; }
    
    [Required]
    public DateTime FechaVencimiento { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 