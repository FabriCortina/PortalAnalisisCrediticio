using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Credito
{
    public class CreditoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El tipo de crédito es requerido")]
        [StringLength(50, ErrorMessage = "El tipo de crédito no puede exceder los 50 caracteres")]
        public required string TipoCredito { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La tasa de interés es requerida")]
        public decimal TasaInteres { get; set; }

        [Required(ErrorMessage = "El plazo es requerido")]
        public int Plazo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres")]
        public required string Estado { get; set; }

        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Relaciones
        public ClienteDTO? Cliente { get; set; }

        public int? SolicitudProductoId { get; set; }
        public DateTime FechaOtorgamiento { get; set; }
        public decimal MontoOtorgado { get; set; }
        public int CantidadCuotas { get; set; }
        public DateTime? FechaUltimoPago { get; set; }
        public decimal SaldoPendiente { get; set; }
        public List<PagoCreditoDTO> Pagos { get; set; }
    }

    public class CreateCreditoDTO
    {
        [Required]
        public int ClienteId { get; set; }
        public int? SolicitudProductoId { get; set; }
        [Required]
        public DateTime FechaOtorgamiento { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal MontoOtorgado { get; set; }
        [Required]
        [Range(1, 120)]
        public int CantidadCuotas { get; set; }
        [Required]
        [StringLength(50)]
        public string Estado { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal TasaInteres { get; set; }
    }

    public class UpdateCreditoDTO
    {
        [Required]
        [StringLength(50)]
        public string Estado { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal TasaInteres { get; set; }
        public DateTime? FechaUltimoPago { get; set; }
        public decimal SaldoPendiente { get; set; }
    }
} 