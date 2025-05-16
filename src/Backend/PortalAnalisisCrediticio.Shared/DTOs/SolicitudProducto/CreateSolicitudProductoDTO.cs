using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.SolicitudProducto;

public class CreateSolicitudProductoDTO
{
    [Required(ErrorMessage = "El ID del cliente es requerido")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "La fecha de solicitud es requerida")]
    public DateTime FechaSolicitud { get; set; }

    [Required(ErrorMessage = "El monto total es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser mayor a 0")]
    public decimal MontoTotal { get; set; }

    [Required(ErrorMessage = "El estado es requerido")]
    [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres")]
    public string Estado { get; set; }

    [Required(ErrorMessage = "El pago inicial es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El pago inicial debe ser mayor o igual a 0")]
    public decimal PagoInicial { get; set; }

    [Required(ErrorMessage = "El porcentaje de financiación es requerido")]
    [Range(0, 100, ErrorMessage = "El porcentaje de financiación debe estar entre 0 y 100")]
    public decimal PorcentajeFinanciacion { get; set; }

    [Required(ErrorMessage = "La cantidad de cuotas es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de cuotas debe ser mayor a 0")]
    public int CantidadCuotas { get; set; }

    [Required(ErrorMessage = "El monto financiado es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto financiado debe ser mayor a 0")]
    public decimal MontoFinanciado { get; set; }

    [Required(ErrorMessage = "El monto de la cuota es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto de la cuota debe ser mayor a 0")]
    public decimal MontoCuota { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "La lista de productos es requerida")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
    public List<CreateProductoSolicitudDTO> Productos { get; set; } = new();
}

public class CreateProductoSolicitudDTO
{
    [Required(ErrorMessage = "El ID del producto es requerido")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "La cantidad es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "El subtotal es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
    public decimal Subtotal { get; set; }
} 