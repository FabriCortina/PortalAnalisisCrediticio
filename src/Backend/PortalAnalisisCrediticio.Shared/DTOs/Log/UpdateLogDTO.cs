using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Log;

public class UpdateLogDTO
{
    [Required]
    public DateTime Fecha { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Nivel { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Categoria { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Mensaje { get; set; }
    
    [MaxLength(500)]
    public string Detalles { get; set; }
    
    [MaxLength(50)]
    public string Usuario { get; set; }
    
    [MaxLength(50)]
    public string Ip { get; set; }
    
    [MaxLength(200)]
    public string Url { get; set; }
    
    [MaxLength(50)]
    public string Metodo { get; set; }
    
    [MaxLength(50)]
    public string Modulo { get; set; }
    
    [MaxLength(50)]
    public string Accion { get; set; }
    
    [MaxLength(500)]
    public string Excepcion { get; set; }
    
    [MaxLength(500)]
    public string StackTrace { get; set; }
    
    [MaxLength(500)]
    public string Parametros { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
} 