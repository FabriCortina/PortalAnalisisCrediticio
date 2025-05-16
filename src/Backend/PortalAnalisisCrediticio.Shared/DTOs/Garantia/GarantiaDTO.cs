using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Garantia;

public class GarantiaDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int? SolicitudProductoId { get; set; }
    public string TipoGarantia { get; set; }
    public decimal ValorEstimado { get; set; }
    public string Descripcion { get; set; }
    public string DocumentoAsociado { get; set; }
    public string Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}

public class CreateGarantiaDTO
{
    [Required]
    public int ClienteId { get; set; }

    public int? SolicitudProductoId { get; set; }

    [Required]
    [StringLength(100)]
    public string TipoGarantia { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorEstimado { get; set; }

    [StringLength(500)]
    public string Descripcion { get; set; }

    [StringLength(200)]
    public string DocumentoAsociado { get; set; }

    [StringLength(100)]
    public string Estado { get; set; }
}

public class UpdateGarantiaDTO
{
    [Required]
    [StringLength(100)]
    public string TipoGarantia { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ValorEstimado { get; set; }

    [StringLength(500)]
    public string Descripcion { get; set; }

    [StringLength(200)]
    public string DocumentoAsociado { get; set; }

    [StringLength(100)]
    public string Estado { get; set; }
} 