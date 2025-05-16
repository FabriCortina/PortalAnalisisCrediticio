using System.ComponentModel.DataAnnotations;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;

namespace PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

public class CreateInformeRiesgoDTO
{
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Titulo { get; set; }
    
    [Required]
    public DateTime FechaInforme { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string NivelRiesgo { get; set; }
    
    [Required]
    [Range(0, 100)]
    public decimal PuntuacionRiesgo { get; set; }
    
    [Required]
    [Range(0, 100)]
    public decimal PuntuacionHistorialCrediticio { get; set; }
    
    [Required]
    [Range(0, 100)]
    public decimal PuntuacionCapacidadPago { get; set; }
    
    [Required]
    [Range(0, 100)]
    public decimal PuntuacionGarantias { get; set; }
    
    [Required]
    [Range(0, 100)]
    public decimal PuntuacionSituacionEconomica { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string AnalisisRiesgo { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string FactoresRiesgo { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Recomendaciones { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    // Relaciones
    public ClienteDTO Cliente { get; set; }
} 