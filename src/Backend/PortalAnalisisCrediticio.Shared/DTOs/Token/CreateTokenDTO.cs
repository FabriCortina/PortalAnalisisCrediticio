using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Token;

public class CreateTokenDTO
{
    [Required]
    public int UsuarioId { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string Token { get; set; }
    
    [Required]
    public DateTime FechaExpiracion { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [MaxLength(50)]
    public string Ip { get; set; }
    
    [MaxLength(200)]
    public string UserAgent { get; set; }
} 