using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Nota;

public class UpdateNotaDTO
{
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Contenido { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoNota { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Categoria { get; set; }
    
    [Required]
    public DateTime FechaCreacion { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Autor { get; set; }
    
    [Required]
    public bool Importante { get; set; }
    
    [Required]
    public bool Privada { get; set; }
    
    [Required]
    public bool Activa { get; set; }
    
    [MaxLength(500)]
    public string Etiquetas { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
    
    [MaxLength(100)]
    public string DocumentoAdjunto { get; set; }
} 