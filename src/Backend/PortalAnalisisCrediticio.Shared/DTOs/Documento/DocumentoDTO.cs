using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Documento;

public class DocumentoDTO
{
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoDocumento { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string NumeroDocumento { get; set; }
    
    [Required]
    public DateTime FechaEmision { get; set; }
    
    public DateTime? FechaVencimiento { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string EntidadEmisora { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string RutaArchivo { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FormatoArchivo { get; set; }
    
    [Required]
    public long TamanioArchivo { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    [Required]
    public DateTime FechaRegistro { get; set; }
    
    public DateTime? FechaActualizacion { get; set; }
    
    public DateTime? FechaEliminacion { get; set; }
    
    [MaxLength(500)]
    public string MotivoEliminacion { get; set; }
} 