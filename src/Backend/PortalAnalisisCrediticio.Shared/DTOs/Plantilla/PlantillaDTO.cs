using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Plantilla;

public class PlantillaDTO
{
    public int Id { get; set; }
    
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
    public string Contenido { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [Required]
    public DateTime FechaRegistro { get; set; }
    
    public DateTime? FechaActualizacion { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [MaxLength(50)]
    public string UsuarioModificacion { get; set; }
    
    [MaxLength(50)]
    public string Version { get; set; }
} 