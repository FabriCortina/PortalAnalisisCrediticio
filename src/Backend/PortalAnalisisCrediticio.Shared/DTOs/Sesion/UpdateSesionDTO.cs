using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Sesion;

public class UpdateSesionDTO
{
    [Required]
    [MaxLength(50)]
    public string Token { get; set; }
    
    public DateTime? FechaFin { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Ip { get; set; }
    
    [MaxLength(200)]
    public string UserAgent { get; set; }
    
    [Required]
    public bool Activa { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [MaxLength(50)]
    public string Dispositivo { get; set; }
    
    [MaxLength(50)]
    public string Navegador { get; set; }
    
    [MaxLength(50)]
    public string SistemaOperativo { get; set; }
} 