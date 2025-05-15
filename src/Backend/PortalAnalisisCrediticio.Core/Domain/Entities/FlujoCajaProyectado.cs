using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class FlujoCajaProyectado
{
    public int Id { get; set; }

    [Required]
    public int InformacionFinancieraId { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Ingresos { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Egresos { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Saldo { get; set; }

    // Relaciones
    public InformacionFinanciera InformacionFinanciera { get; set; }
} 