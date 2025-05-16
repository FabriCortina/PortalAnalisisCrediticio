using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class ProductoSolicitud
{
    public int Id { get; set; }

    [Required]
    public int ProductoId { get; set; }

    [Required]
    public int SolicitudProductoId { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; set; }

    // Relaciones
    public Producto Producto { get; set; }
    public SolicitudProducto SolicitudProducto { get; set; }
} 