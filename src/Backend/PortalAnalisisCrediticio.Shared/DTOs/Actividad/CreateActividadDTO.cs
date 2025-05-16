using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Actividad;

public class CreateActividadDTO
{
    [Required]
    public string UsuarioId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Accion { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Detalle { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string IpAddress { get; set; }
    
    [Required]
    public DateTime Fecha { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoActividad { get; set; }
} 