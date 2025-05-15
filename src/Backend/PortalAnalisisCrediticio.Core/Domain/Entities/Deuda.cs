using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Deuda
{
    public int Id { get; set; }

    [Required]
    public int InformacionFinancieraId { get; set; }

    [Required]
    [MaxLength(100)]
    public string EntidadFinanciera { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    [Required]
    public DateTime FechaVencimiento { get; set; }

    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }

    // Relaciones
    public InformacionFinanciera InformacionFinanciera { get; set; }
} 