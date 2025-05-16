using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Documento;

public class CreateDocumentoDTO
{
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
    [Range(1, long.MaxValue)]
    public long TamanioArchivo { get; set; }
    
    [Required]
    public bool Activo { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 