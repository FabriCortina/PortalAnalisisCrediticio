using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Garantia
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
    [StringLength(100)]
    public string TipoGarantia { get; set; }

    [Required]
    public decimal ValorEstimado { get; set; }

    [StringLength(500)]
    public string Descripcion { get; set; }

    [StringLength(200)]
    public string DocumentoAsociado { get; set; }

    [StringLength(100)]
    public string Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
} 