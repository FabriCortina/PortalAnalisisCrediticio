using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class EstadoFinanciero
{
    public int Id { get; set; }

    [Required]
    public int InformacionFinancieraId { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Activos { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Pasivos { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PatrimonioNeto { get; set; }

    // Relaciones
    public InformacionFinanciera InformacionFinanciera { get; set; }
} 