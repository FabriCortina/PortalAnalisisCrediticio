using System.ComponentModel.DataAnnotations;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;

namespace PortalAnalisisCrediticio.Shared.DTOs.Informes;

public class CreateInformeDTO
{
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    public int SolicitudProductoId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Titulo { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Tipo { get; set; }
    
    [Required]
    public DateTime FechaInforme { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string NivelRiesgo { get; set; }
    
    [Required]
    [Range(0, 100)]
    public decimal PuntuacionRiesgo { get; set; }
    
    [Required]
    public decimal MontoRecomendado { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Moneda { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Conclusiones { get; set; }
    
    [Required]
    [MaxLength(2000)]
    public string Recomendaciones { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    // Relaciones
    public ClienteDTO Cliente { get; set; }
    public SolicitudProductoDTO SolicitudProducto { get; set; }
} 