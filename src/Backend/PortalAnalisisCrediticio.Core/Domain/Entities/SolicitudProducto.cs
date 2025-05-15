using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class SolicitudProducto
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Producto { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoTotal { get; set; }

    [Required]
    [MaxLength(50)]
    public string Estado { get; set; }

    [Required]
    public DateTime FechaSolicitud { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PagoInicial { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal PorcentajeFinanciacion { get; set; }

    [Required]
    public int CantidadCuotas { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoFinanciado { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal MontoCuota { get; set; }

    // Relaciones
    public Cliente Cliente { get; set; }
} 