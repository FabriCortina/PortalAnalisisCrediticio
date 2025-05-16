using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

public class UpdateInformacionFinancieraDTO
{
    [Required]
    [Range(0, double.MaxValue)]
    public decimal IngresosMensuales { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal GastosMensuales { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal PatrimonioNeto { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Activos { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Pasivos { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Moneda { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 