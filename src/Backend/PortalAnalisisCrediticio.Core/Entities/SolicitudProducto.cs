using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Core.Entities;

public class SolicitudProducto
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public string Codigo { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Precio { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required]
    public int CantidadCuotas { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal PorcentajeFinanciacion { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PagoInicial { get; set; }

    public string Observaciones { get; set; }

    public DateTime FechaSolicitud { get; set; } = DateTime.Now;

    public string Estado { get; set; } = "Pendiente";

    public decimal MontoTotal => Precio * Cantidad;

    public decimal MontoFinanciado => MontoTotal * (PorcentajeFinanciacion / 100);

    public decimal MontoCuota => CantidadCuotas > 0 ? MontoFinanciado / CantidadCuotas : 0;
} 