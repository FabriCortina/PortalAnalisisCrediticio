using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class CondicionPago
{
    public int Id { get; set; }

    [Required]
    public int SolicitudProductoId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PagoInicial { get; set; }

    [Required]
    public int CantidadCuotas { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal TasaAplicada { get; set; }

    [Required]
    [MaxLength(3)]
    public string Moneda { get; set; }

    [Required]
    [MaxLength(50)]
    public string FormaPago { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoFinanciado { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoCuota { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; }

    // Relaciones
    public SolicitudProducto SolicitudProducto { get; set; }
} 