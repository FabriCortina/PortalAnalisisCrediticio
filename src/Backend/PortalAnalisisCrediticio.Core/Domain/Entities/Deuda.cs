using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Deuda
{
    public int Id { get; set; }

    [Required]
    public int InformacionFinancieraId { get; set; }

    [Required]
    [MaxLength(50)]
    public string TipoDeuda { get; set; }

    [Required]
    [MaxLength(100)]
    public string EntidadFinanciera { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TasaInteres { get; set; }

    [Required]
    public DateTime FechaVencimiento { get; set; }

    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }

    [Required]
    [MaxLength(50)]
    public string Moneda { get; set; }

    [Required]
    [MaxLength(100)]
    public string FuenteInformacion { get; set; }

    [MaxLength(255)]
    public string Observaciones { get; set; }

    // Relaciones
    public InformacionFinanciera InformacionFinanciera { get; set; }
} 