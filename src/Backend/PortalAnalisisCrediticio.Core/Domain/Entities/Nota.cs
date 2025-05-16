using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Nota
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
    public string Titulo { get; set; }

    [Required]
    [StringLength(2000)]
    public string Contenido { get; set; }

    [Required]
    [StringLength(100)]
    public string Autor { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
} 