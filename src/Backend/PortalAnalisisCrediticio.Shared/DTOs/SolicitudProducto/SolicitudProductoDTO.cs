using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.SolicitudProducto;

public class SolicitudProductoDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public decimal MontoTotal { get; set; }
    public string Estado { get; set; }
    public decimal PagoInicial { get; set; }
    public decimal PorcentajeFinanciacion { get; set; }
    public int CantidadCuotas { get; set; }
    public decimal MontoFinanciado { get; set; }
    public decimal MontoCuota { get; set; }
    public string? Observaciones { get; set; }
    public List<ProductoSolicitudDTO> Productos { get; set; } = new();
}

public class ProductoSolicitudDTO
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; }
    public string ProductoCodigoInterno { get; set; }
    public decimal PrecioUnitario { get; set; }
    public string Moneda { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal { get; set; }
} 