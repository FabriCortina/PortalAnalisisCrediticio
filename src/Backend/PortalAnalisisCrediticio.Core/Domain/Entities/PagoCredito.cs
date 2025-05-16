using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class PagoCredito
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CreditoId { get; set; }
    [ForeignKey("CreditoId")]
    public Credito Credito { get; set; }

    [Required]
    public DateTime FechaPago { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoPagado { get; set; }

    [Required]
    public int NumeroCuota { get; set; }

    [Required]
    public DateTime VencimientoCuota { get; set; }

    [Required]
    [StringLength(30)]
    public string EstadoCuota { get; set; } // pagada, vencida, pendiente
} 