using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Producto;

public class SolicitudProductoDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Producto { get; set; }
    
    [Required]
    public int Cantidad { get; set; }
    
    [Required]
    public decimal MontoTotal { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }
} 