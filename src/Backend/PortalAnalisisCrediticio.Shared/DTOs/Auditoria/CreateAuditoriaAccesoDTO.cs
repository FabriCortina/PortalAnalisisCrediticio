using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Auditoria;

public class CreateAuditoriaAccesoDTO
{
    [Required]
    public int UsuarioId { get; set; }
    
    [Required]
    public DateTime FechaAcceso { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoAccion { get; set; }
    
    [MaxLength(200)]
    public string Descripcion { get; set; }
    
    [MaxLength(50)]
    public string IP { get; set; }
    
    [MaxLength(200)]
    public string UserAgent { get; set; }
} 