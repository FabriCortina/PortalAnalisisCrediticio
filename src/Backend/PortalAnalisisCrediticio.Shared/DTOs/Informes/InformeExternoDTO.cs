using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Informes;

public class InformeExternoDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Fuente { get; set; }
    
    [Required]
    public DateTime FechaInforme { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoInforme { get; set; }
    
    [Required]
    public string Contenido { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
} 