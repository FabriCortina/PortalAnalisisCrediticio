using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Credito
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }
    [ForeignKey("ClienteId")]
    public Cliente Cliente { get; set; }

    public int? SolicitudProductoId { get; set; }
    [ForeignKey("SolicitudProductoId")]
    public SolicitudProducto SolicitudProducto { get; set; }

    [Required]
    public DateTime FechaOtorgamiento { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoOtorgado { get; set; }

    [Required]
    public int CantidadCuotas { get; set; }

    [Required]
    [StringLength(50)]
    public string Estado { get; set; } // vigente, cancelado, en mora, etc.

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal TasaInteres { get; set; }

    public DateTime? FechaUltimoPago { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SaldoPendiente { get; set; }

    public ICollection<PagoCredito> Pagos { get; set; }
} 