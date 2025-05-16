using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Notificacion;

public class UpdateNotificacionRequestDTO
{
    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; }
    
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
    public bool Activa { get; set; }
    
    public DateTime? FechaEnvio { get; set; }
    
    public DateTime? FechaLeida { get; set; }
    
    [MaxLength(50)]
    public string Destinatario { get; set; }
    
    [MaxLength(50)]
    public string Remitente { get; set; }
    
    [MaxLength(50)]
    public string Prioridad { get; set; }
    
    public bool? RequiereConfirmacion { get; set; }
    
    public bool? Confirmada { get; set; }
    
    public DateTime? FechaConfirmacion { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
    
    public int? Version { get; set; }
} 