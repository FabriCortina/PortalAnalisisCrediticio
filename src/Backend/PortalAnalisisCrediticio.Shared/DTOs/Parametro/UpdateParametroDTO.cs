using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Parametro;

public class UpdateParametroDTO
{
    [Required]
    [MaxLength(50)]
    public string Codigo { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Descripcion { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    public string Valor { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
} 