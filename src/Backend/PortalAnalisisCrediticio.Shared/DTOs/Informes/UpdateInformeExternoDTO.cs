using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Informes;

public class UpdateInformeExternoDTO
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Fuente { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string TipoInforme { get; set; }
    
    [Required]
    public string Contenido { get; set; }
    
    [Required]
    public DateTime FechaInforme { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
} 