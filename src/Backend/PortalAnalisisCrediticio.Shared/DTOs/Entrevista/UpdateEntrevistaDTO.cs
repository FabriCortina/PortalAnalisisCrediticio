using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Entrevista;

public class UpdateEntrevistaDTO
{
    [Required]
    public DateTime FechaEntrevista { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Entrevistador { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoEntrevista { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Lugar { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Objetivo { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Desarrollo { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Conclusiones { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
    
    [Required]
    public bool Activa { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    public DateTime? FechaCancelacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoCancelacion { get; set; }
    
    [MaxLength(100)]
    public string DocumentoAdjunto { get; set; }
} 