using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Financiero;

public class DeudaDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string EntidadFinanciera { get; set; }
    
    [Required]
    public decimal Monto { get; set; }
    
    [Required]
    public DateTime FechaVencimiento { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
} 