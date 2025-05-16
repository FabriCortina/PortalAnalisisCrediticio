using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.CondicionPago;

public class CondicionPagoDTO
{
    public int Id { get; set; }
    public int SolicitudProductoId { get; set; }

    [Required(ErrorMessage = "El pago inicial es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El pago inicial debe ser mayor o igual a 0")]
    public decimal PagoInicial { get; set; }

    [Required(ErrorMessage = "La cantidad de cuotas es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de cuotas debe ser mayor a 0")]
    public int CantidadCuotas { get; set; }

    [Required(ErrorMessage = "La tasa aplicada es requerida")]
    [Range(0, 100, ErrorMessage = "La tasa aplicada debe estar entre 0 y 100")]
    public decimal TasaAplicada { get; set; }

    [Required(ErrorMessage = "La moneda es requerida")]
    [MaxLength(3, ErrorMessage = "La moneda debe tener 3 caracteres")]
    public string Moneda { get; set; }

    [Required(ErrorMessage = "La forma de pago es requerida")]
    [MaxLength(50, ErrorMessage = "La forma de pago no puede exceder los 50 caracteres")]
    public string FormaPago { get; set; }

    [Required(ErrorMessage = "El monto financiado es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto financiado debe ser mayor a 0")]
    public decimal MontoFinanciado { get; set; }

    [Required(ErrorMessage = "El monto de la cuota es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto de la cuota debe ser mayor a 0")]
    public decimal MontoCuota { get; set; }

    [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}

public class CreateCondicionPagoDTO
{
    [Required(ErrorMessage = "El pago inicial es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El pago inicial debe ser mayor o igual a 0")]
    public decimal PagoInicial { get; set; }

    [Required(ErrorMessage = "La cantidad de cuotas es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de cuotas debe ser mayor a 0")]
    public int CantidadCuotas { get; set; }

    [Required(ErrorMessage = "La tasa aplicada es requerida")]
    [Range(0, 100, ErrorMessage = "La tasa aplicada debe estar entre 0 y 100")]
    public decimal TasaAplicada { get; set; }

    [Required(ErrorMessage = "La moneda es requerida")]
    [MaxLength(3, ErrorMessage = "La moneda debe tener 3 caracteres")]
    public string Moneda { get; set; }

    [Required(ErrorMessage = "La forma de pago es requerida")]
    [MaxLength(50, ErrorMessage = "La forma de pago no puede exceder los 50 caracteres")]
    public string FormaPago { get; set; }

    [Required(ErrorMessage = "El monto financiado es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto financiado debe ser mayor a 0")]
    public decimal MontoFinanciado { get; set; }

    [Required(ErrorMessage = "El monto de la cuota es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto de la cuota debe ser mayor a 0")]
    public decimal MontoCuota { get; set; }

    [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
    public string? Observaciones { get; set; }
}

public class UpdateCondicionPagoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El pago inicial es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El pago inicial debe ser mayor o igual a 0")]
    public decimal PagoInicial { get; set; }

    [Required(ErrorMessage = "La cantidad de cuotas es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de cuotas debe ser mayor a 0")]
    public int CantidadCuotas { get; set; }

    [Required(ErrorMessage = "La tasa aplicada es requerida")]
    [Range(0, 100, ErrorMessage = "La tasa aplicada debe estar entre 0 y 100")]
    public decimal TasaAplicada { get; set; }

    [Required(ErrorMessage = "La moneda es requerida")]
    [MaxLength(3, ErrorMessage = "La moneda debe tener 3 caracteres")]
    public string Moneda { get; set; }

    [Required(ErrorMessage = "La forma de pago es requerida")]
    [MaxLength(50, ErrorMessage = "La forma de pago no puede exceder los 50 caracteres")]
    public string FormaPago { get; set; }

    [Required(ErrorMessage = "El monto financiado es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto financiado debe ser mayor a 0")]
    public decimal MontoFinanciado { get; set; }

    [Required(ErrorMessage = "El monto de la cuota es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto de la cuota debe ser mayor a 0")]
    public decimal MontoCuota { get; set; }

    [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
    public string? Observaciones { get; set; }
} 