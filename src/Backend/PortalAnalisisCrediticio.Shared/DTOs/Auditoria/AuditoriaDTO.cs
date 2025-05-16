using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Auditoria;

public class AuditoriaDTO
{
    public int Id { get; set; }
    
    [Required]
    public DateTime Fecha { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Usuario { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Accion { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Entidad { get; set; }
    
    [Required]
    public int EntidadId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Ip { get; set; }
    
    [MaxLength(200)]
    public string Url { get; set; }
    
    [MaxLength(50)]
    public string Metodo { get; set; }
    
    [MaxLength(500)]
    public string DatosAnteriores { get; set; }
    
    [MaxLength(500)]
    public string DatosNuevos { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 