using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

public class CreateFlujoCajaProyectadoDTO
{
    [Required]
    public int InformacionFinancieraId { get; set; }
    
    [Required]
    public DateTime Fecha { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Ingresos { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Egresos { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Saldo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 