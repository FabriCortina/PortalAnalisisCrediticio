using System;
using System.ComponentModel.DataAnnotations;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;

namespace PortalAnalisisCrediticio.Shared.DTOs.Producto;

public class SolicitudProductoDTO
{
    public int Id { get; set; }
    
    [Required]
    public int ClienteId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string TipoProducto { get; set; }
    
    [Required]
    public decimal MontoSolicitado { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Moneda { get; set; }
    
    [Required]
    public int PlazoMeses { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
    
    [Required]
    public DateTime FechaSolicitud { get; set; }
    
    public DateTime? FechaAprobacion { get; set; }
    
    public DateTime? FechaRechazo { get; set; }
    
    [MaxLength(500)]
    public string MotivoRechazo { get; set; }
    
    [MaxLength(1000)]
    public string Descripcion { get; set; }
    
    [MaxLength(500)]
    public string Observaciones { get; set; }
    
    // Relaciones
    public ClienteDTO Cliente { get; set; }
} 