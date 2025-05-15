using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs;

public class SolicitudProductoDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Nombre { get; set; }
    public string Codigo { get; set; }
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public int CantidadCuotas { get; set; }
    public decimal PorcentajeFinanciacion { get; set; }
    public decimal PagoInicial { get; set; }
    public string Observaciones { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal MontoFinanciado { get; set; }
    public decimal MontoCuota { get; set; }
}

public class CreateSolicitudProductoDTO
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(50)]
    public string Codigo { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Precio { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required]
    [Range(1, 60)]
    public int CantidadCuotas { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal PorcentajeFinanciacion { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PagoInicial { get; set; }

    public string Observaciones { get; set; }
}

public class UpdateSolicitudProductoDTO
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; }

    [Required]
    [StringLength(50)]
    public string Codigo { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Precio { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required]
    [Range(1, 60)]
    public int CantidadCuotas { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal PorcentajeFinanciacion { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PagoInicial { get; set; }

    public string Observaciones { get; set; }

    [Required]
    public string Estado { get; set; }
} 