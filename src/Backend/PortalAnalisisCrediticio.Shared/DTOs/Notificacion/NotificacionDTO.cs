using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Notificacion;

public class NotificacionDTO
{
    public int Id { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Titulo { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Mensaje { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Prioridad { get; set; }
    
    [Required]
    public bool Leida { get; set; }
    
    public DateTime? FechaLectura { get; set; }
    
    [Required]
    public DateTime FechaCreacion { get; set; }
    
    public DateTime? FechaExpiracion { get; set; }
    
    [MaxLength(200)]
    public string Url { get; set; }
    
    [MaxLength(50)]
    public string Modulo { get; set; }
    
    [MaxLength(50)]
    public string Accion { get; set; }
    
    [MaxLength(500)]
    public string DatosAdicionales { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 